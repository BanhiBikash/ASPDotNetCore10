import React, { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import api from '../api/axiosConfig'; // Ensure this points correctly to your configured axios instance/interceptor

const Checkout = () => {
  const location = useLocation();
  const navigate = useNavigate();

  // 🎯 1. Safely extract the passed orderData payload or fallback to defaults
  const orderData = location.state?.orderData || { items: [], totalAmount: 0 };
  const { items, totalAmount } = orderData;

  // 🎯 2. Local state to capture user profile information from the API endpoint
  const [userProfile, setUserProfile] = useState({
    name: 'Customer',
    address: '',
    city: '',
    state: '',
    postalCode: '',
    country: '',
    isLoading: true
  });

  // 🎯 3. Fetch data from your backend endpoint on mount
  useEffect(() => {
    const fetchProfileDetails = async () => {
      const token = localStorage.getItem('token');
      if (!token) {
        setUserProfile(prev => ({ ...prev, isLoading: false }));
        return;
      }

      try {
        const response = await api.get('/v1/Account/getprofiledetails', {
          headers: {
            'Authorization': `Bearer ${token}`
          }
        });

        const data = response.data;
        
        // Assemble name safely out of your backend's firstName and lastName properties
        const fullName = data.firstName && data.lastName 
          ? `${data.firstName} ${data.lastName}` 
          : 'Customer';

        setUserProfile({
          name: fullName,
          address: data.address || 'No primary address configured.',
          city: data.city || '',
          state: data.state || '',
          postalCode: data.postalCode || '',
          country: data.country || '',
          isLoading: false
        });
      } catch (err) {
        console.error("Failed to load user address information from endpoint:", err);
        setUserProfile(prev => ({ ...prev, isLoading: false }));
      }
    };

    fetchProfileDetails();
  }, []);

  // 🎯 4. Submit fully constructed payload straight to your Order Controller
  const handlePaymentSubmit = async () => {
    const completedBackendPayload = {
      shippingAddress: userProfile.address,
      postalCode: userProfile.postalCode,
      city: userProfile.city,
      country: userProfile.country,
      items: items.map(item => ({
        productId: item.productId,
        productName: item.productName,
        imageUrl: item.imageUrl,
        quantity: item.quantity,
        unitPrice: item.unitPrice
      }))
    };

    try {
      console.log("Submitting order payload to backend:", completedBackendPayload);
      const response = await api.post('/v1/orders/ReceiveOrder', completedBackendPayload);
      
      if (response.status === 200 || response.status === 201) {
         navigate('/order-success');
      }
    } catch (error) {
      console.error("Order processing encountered an issue:", error);
    }
  };

  if (userProfile.isLoading) {
    return (
      <div style={styles.emptyContainer}>
        <h3>Loading your delivery details...</h3>
      </div>
    );
  }

  if (!items || items.length === 0) {
    return (
      <div style={styles.emptyContainer}>
        <h3>Your checkout context is empty.</h3>
        <button style={styles.primaryBtn} onClick={() => navigate('/')}>Return to Shopping</button>
      </div>
    );
  }

  return (
    <div style={styles.container}>
      <h2 style={styles.heading}>Review Your Order</h2>

      {/* 🏡 SECTION A: Shipping Address Card Context */}
      <div style={styles.addressCard}>
        <div style={styles.addressHeader}>
          <h3 style={styles.sectionTitle}>Shipping Address</h3>
          <button 
            style={styles.linkBtn} 
            onClick={() => navigate('/account_update')}
          >
            Change Address
          </button>
        </div>
        <p style={styles.userName}>{userProfile.name}</p>
        <p style={styles.addressText}>{userProfile.address}</p>
        {(userProfile.city || userProfile.state || userProfile.postalCode) && (
          <p style={styles.addressText}>
            {userProfile.city}
            {userProfile.state && `, ${userProfile.state}`}
            {userProfile.postalCode && ` ${userProfile.postalCode}`}
          </p>
        )}
        {userProfile.country && <p style={styles.addressText}>{userProfile.country}</p>}
      </div>

      {/* 📦 SECTION B: Flat Order Summary List */}
      <div style={styles.itemsCard}>

      {/* 💰 TOP BUTTON: Quick Pay Action Row */}
      <div style={styles.actionRow}>
        <button style={styles.paymentBtn} onClick={handlePaymentSubmit}>
          Pay ₹{totalAmount.toLocaleString('en-IN')}
        </button>
      </div>

      {/* itemslist */}
        <h3 style={styles.sectionTitle}>Review Items</h3>
        <div style={styles.itemsList}>
          {items.map((item) => (
            <div key={item.productId} style={styles.itemRow}>
              <img 
                src={item.imageUrl} 
                alt={item.productName} 
                style={styles.productImg} 
                onError={(e) => { e.target.src = 'https://via.placeholder.com/80?text=Product'; }}
              />
              <div style={styles.itemDetails}>
                <h4 style={styles.productName}>{item.productName}</h4>
                <p style={styles.productMeta}>Qty: <strong>{item.quantity}</strong></p>
                <p style={styles.productPrice}>₹{item.unitPrice} each</p>
              </div>
            </div>
          ))}
        </div>

      {/* 💰 BOTTOM BUTTON: Standard Flow Pay Button (Not Sticky) */}
      <div style={styles.bottomActionRow}>
        <button style={styles.paymentBtn} onClick={handlePaymentSubmit}>
          Pay ₹{totalAmount.toLocaleString('en-IN')}
        </button>
      </div>
      </div>
    </div>
  );
};

/* Clean Embedded View-Port Styles */
const styles = {
  container: {
    maxWidth: '650px',
    margin: '20px auto',
    padding: '0 15px 40px 15px', // Normal padding since there's no fixed footer overlap
    fontFamily: '-apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif',
  },
  heading: {
    fontSize: '24px',
    fontWeight: '600',
    color: '#111',
    marginBottom: '20px',
  },
  addressCard: {
    background: '#fff',
    border: '1px solid #ddd',
    borderRadius: '8px',
    padding: '16px',
    marginBottom: '16px',
    boxShadow: '0 1px 3px rgba(0,0,0,0.05)',
  },
  addressHeader: {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: '12px',
    borderBottom: '1px solid #f0f0f0',
    paddingBottom: '8px',
  },
  sectionTitle: {
    fontSize: '16px',
    fontWeight: '600',
    color: '#222',
    margin: 0,
  },
  linkBtn: {
    background: 'none',
    border: 'none',
    color: '#0066cc',
    fontSize: '14px',
    fontWeight: '500',
    cursor: 'pointer',
    padding: 0,
  },
  userName: {
    fontWeight: '600',
    fontSize: '15px',
    margin: '0 0 4px 0',
    color: '#333',
  },
  addressText: {
    margin: '0 0 2px 0',
    color: '#555',
    fontSize: '14px',
    lineHeight: '1.4',
  },
  actionRow: {
    width: '100%',
    textAlign: 'center',
    marginBottom: '20px',
  },
  itemsCard: {
    background: '#fff',
    border: '1px solid #ddd',
    borderRadius: '8px',
    padding: '16px',
    boxShadow: '0 1px 3px rgba(0,0,0,0.05)',
  },
  itemsList: {
    marginTop: '12px',
  },
  itemRow: {
    display: 'flex',
    alignItems: 'center',
    padding: '12px 0',
    borderBottom: '1px solid #eee',
  },
  productImg: {
    width: '70px',
    height: '70px',
    objectFit: 'contain',
    borderRadius: '4px',
    border: '1px solid #f0f0f0',
    marginRight: '16px',
    background: '#fafafa',
  },
  itemDetails: {
    flex: 1,
  },
  productName: {
    margin: '0 0 4px 0',
    fontSize: '15px',
    fontWeight: '500',
    color: '#111',
  },
  productMeta: {
    margin: '0 0 2px 0',
    fontSize: '13px',
    color: '#666',
  },
  productPrice: {
    margin: 0,
    fontSize: '14px',
    fontWeight: '600',
    color: '#333',
  },
  bottomActionRow: {
    width: '100%',
    textAlign: 'center',
    marginTop: '24px', // Space out cleanly below the item summary card block
  },
  paymentBtn: {
    background: '#ff9900', 
    border: '1px solid #a88734',
    borderRadius: '4px',
    width: '100%',
    maxWidth: '400px',
    padding: '12px 0',
    fontSize: '16px',
    fontWeight: '600',
    color: '#111',
    cursor: 'pointer',
  },
  emptyContainer: {
    textAlign: 'center',
    marginTop: '50px',
    padding: '20px',
  },
  primaryBtn: {
    background: '#f0c14b',
    border: '1px solid #a88734',
    padding: '8px 16px',
    borderRadius: '4px',
    cursor: 'pointer',
  }
};

export default Checkout;