import React, { useState, useEffect } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import logo from "../assets/Amazon-Logo.png"
import api from '../api/axiosConfig'; // Ensure this points correctly to your configured axios instance/interceptor

const Checkout = () => {
  const location = useLocation();
  const navigate = useNavigate();

  // 🎯 1. Safely extract the passed orderData payload or fallback to defaults
  const orderData = location.state?.orderData || { items: [], totalAmount: 0 };
  const { items, totalAmount } = orderData;

  // 🎯 2. Local state to capture user profile information from the API endpoint
  const [userProfile, setUserProfile] = useState({
    userId: '', // 🚀 Track the raw Guid UserId safely for the payload contract
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
          userId: data.userId || '', // 🚀 Store the backend generated Guid UserId value securely
          name: fullName,
          address: data.address || '',
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

  //razorpay helper function
  const loadRazorpayScript = () => {
    return new Promise((resolve) => {
      const script = document.createElement('script');
      script.src = 'https://checkout.razorpay.com/v1/checkout.js';
      script.async = true;
      script.onload = () => resolve(true);
      script.onerror = () => resolve(false);
      document.body.appendChild(script);
    });
  };

  // Submit fully constructed payload straight to Order Controller
  const handlePaymentSubmit = async () => {

    // Load the script check
    const isScriptLoaded = await loadRazorpayScript();
    if (!isScriptLoaded) {
      alert("Razorpay SDK failed to load. Are you online?");
      return;
    }

    // The backend now extracts userid safely from JWT Bearer token claims.
    const completedBackendPayload = {
      ShippingAddress: userProfile.address || "No primary address configured.",
      PostalCode: userProfile.postalCode || "000000",
      City: userProfile.city || "Default City",
      Country: userProfile.country || "India",

      // Mapped items matching your OrderItem schema requirements precisely
      Items: items.map(item => ({
        ProductId: item.productId,
        ProductName: item.productName || "Product Name",
        ImageUrl: item.imageUrl || "https://via.placeholder.com/80?text=Product",
        Quantity: parseInt(item.quantity, 10) || 1,
        UnitPrice: parseFloat(item.unitPrice) || 0
      }))
    };

    try {
      console.log("Submitting secure payload to backend:", completedBackendPayload);
      const response = await api.post('/v1/orders/ReceiveOrder', completedBackendPayload);

      //order is successfull go to transaction
      if (response.status === 200 || response.status === 201) {

        //extract orderId and amount
        const { id, totalAmount } = response.data;
        console.log(response.data)

        //check if the details are valid, amount>0 and valid guid order id
        if (totalAmount > 0 && /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i.test(id)) {

          //divert to razorpay
          console.log("Phase 2: Initiating Razorpay Gateway UI...");
          const options = {
            key: "rzp_test_SxxtiUPuMmx0Iy", // Replace with your Test/Live Key ID from Razorpay Dashboard
            amount: totalAmount * 100, // Razorpay expects amount in paisa (e.g., ₹500 = 50000 paisa)
            currency: "INR",
            name: "Amazon Clone by Banhi",
            description: `Payment for Order #${id}`,
            image: "https://www.bing.com/images/search?view=detailV2&ccid=bvdmZIag&id=1205845242B5E5416E8D660E151D97BCCB2B33E1&thid=OIP.bvdmZIaghz-ThwkCLvyc4wHaFs&mediaurl=https%3a%2f%2fi.pinimg.com%2foriginals%2f06%2f28%2f7e%2f06287ea10082eb312719c7b5fc4456df.png&exph=591&expw=768&q=amazon&FORM=IRPRST&ck=129CD779CF8948FDD685F2BFBEE5710A&selectedIndex=2&itb=0",

            // Hand over the unique database OrderId to track inside Razorpay dashboard metadata
            notes: {
              company_order_id: id
            },
            prefill: {
              name: userProfile.name,
              // email and contact can be added here if available in your userProfile state
            },
            theme: {
              color: "#ff9900" // Matches your current Amazon orange color accents
            },

            // Callback executed instantly when bank authentication succeeds
            handler: async function (razorpayResponse) {
              console.log("Razorpay payment captured successfully:", razorpayResponse);

              // 🎯 PHASE 3: Send confirmation credentials back to the backend to finalize
              const confirmationPayload = {
                OrderId: id,
                RazorpayPaymentId: razorpayResponse.razorpay_payment_id,
                RazorpayOrderId: razorpayResponse.razorpay_order_id || "",
                RazorpaySignature: razorpayResponse.razorpay_signature,
                // 🎯 Pass the instrument method string down to your new backend property
                // If your frontend checkout script tracks whether the user selected card or upi, map it here.
                // Otherwise, 'UPI' or 'Card' can be safely specified depending on your local choice state tracker.
                PaymentMethod: "UPI"
              };

              try {
                console.log("Phase 3: Confirming payment with application backend...");
                const confirmResponse = await api.post('/v1/Transaction/ConfirmPayment', confirmationPayload);

                if (confirmResponse.status === 200 || confirmResponse.status === 201) {
                  navigate('/order-success');
                } else {
                  navigate('/');
                }
              } catch (confirmError) {
                console.error("Backend validation failed:", confirmError);
                alert("Transaction auditing sequence failure.");
                navigate('/cart');
              }
            }
          };

          const rzp = new window.Razorpay(options);

          // Handle payment dismissal/failures gracefully
          rzp.on('payment.failed', function (failedResponse) {
            console.error("Payment gate transaction terminated:", failedResponse.error);
            alert(`Payment Failed: ${failedResponse.error.description}`);
          });

          rzp.open();
        }
      }
    } catch (error) {
      console.error("Order workflow execution crashed:", error);
      alert("Checkout Halted: " + (error.response?.data || "Stock allocation issue or network failure."));
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
        <p style={styles.addressText}>{userProfile.address || 'No primary address configured.'}</p>
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
                onError={(e) => { e.target.src = ''; }}
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
    padding: '0 15px 40px 15px',
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
    marginTop: '24px',
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