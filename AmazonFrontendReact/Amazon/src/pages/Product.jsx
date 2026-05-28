import React, { useState, useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axiosConfig';
import UserContext from '../context/UserContext'; // 🌐 Consume global identity framework context

const Product = () => {
  const { user } = useContext(UserContext); // Access logged-in state structure
  const navigate = useNavigate();
  const [products, setProducts] = useState([]);
  const [uiStatus, setUiStatus] = useState({ loading: true, error: null });
  const [actionLoading, setActionLoading] = useState({}); // Tracks loading states per specific product item button click

  // 📡 Fetch catalog array on component mount
  useEffect(() => {
    const fetchCatalog = async () => {
      try {
        const response = await api.get('/v1/Products');
        const catalogData = Array.isArray(response.data) ? response.data : [];
        setProducts(catalogData);
        setUiStatus({ loading: false, error: null });
      } catch (err) {
        console.error('Handshake catalog error context:', err);
        const backendErrorMessage = err.response?.data || err.message || 'Failed to sync product inventory.';
        setUiStatus({
          loading: false,
          error: typeof backendErrorMessage === 'string' ? backendErrorMessage : 'Database service context offline.'
        });
      }
    };

    fetchCatalog();
  }, []);

  // 🛒 Handle Add to Cart Strategy (Supports Database Sync OR LocalStorage Fallback)
  const handleAddToCart = async (product, silent = false) => {
    const productId = product.id;
    setActionLoading(prev => ({ ...prev, [productId]: true }));

    // 🔐 CASE A: User is logged in -> Stream directly to Database pipeline layer
    if (user && user.id) {
      try {
        const payload = {
          productId: productId,
          quantity: 1 // Default to incrementing by 1
        };

        await api.post(`/v1/Cart/UpdateCart?userId=${user.id}`, payload);

        if (!silent) {
          alert(`Successfully added "${product.name}" to your account cart!`);
        }
        return true;
      } catch (err) {
        console.error('Cart operation failure context:', err);
        alert(err.response?.data || 'Failed to update shopping cart allocation.');
        return false;
      } finally {
        setActionLoading(prev => ({ ...prev, [productId]: false }));
      }
    }

    // 👥 CASE B: User is not logged in -> Fallback to LocalStorage Guest Cart
    else {
      try {
        // Retrieve existing guest cart collection or initialize a clean array
        const localCartRaw = localStorage.getItem('guest_cart');
        let guestCart = localCartRaw ? JSON.parse(localCartRaw) : [];

        // Check if item metadata structure is already assigned inside the collection
        const existingItemIndex = guestCart.findIndex(item => item.productId === productId);

        if (existingItemIndex !== -1) {
          // Increment its allocation count matrix safely
          guestCart[existingItemIndex].quantity += 1;
        } else {
          // Append a fresh custom Cartesian line DTO mirror block representation
          guestCart.push({
            productId: product.id,
            productName: product.name,
            price: product.price,
            imageUrl: product.imageUrl,
            category: product.category,
            quantity: 1
          });
        }

        // Commit state updates right back onto browser cache parameters
        localStorage.setItem('guest_cart', JSON.stringify(guestCart));

        // 🔔 Dispatches a custom window storage event object to alert Navbar component counts instantly 
        window.dispatchEvent(new Event('storage'));

        if (!silent) {
          alert(`"${product.name}" added to guest cart! (Saved to local storage)`);
        }
        return true;
      } catch (err) {
        console.error('Local storage cart operation exception context:', err);
        alert('Failed to update local guest cart matrix space.');
        return false;
      } finally {
        setActionLoading(prev => ({ ...prev, [productId]: false }));
      }
    }
  };

  // ⚡ Handle Buy Now (Direct express checkout navigation without modifying the cart)
  const handleBuyNow = (product) => {
    if (!user || !user.id) {
      // If you want to allow guest checkout via localStorage, you can skip this check
      alert('Authentication required. Please log in to complete an express purchase.');
      navigate('/login');
      return;
    }

    // 🎯 Bypasses the cart endpoint entirely and passes the product object via location state
    navigate(`/Checkout/${product.id}`, { state: { directPurchaseItem: product } });
  };

  //the page is loading
  if (uiStatus.loading) {
    return (
      <div className="auth-page-container" style={{ justifyContent: 'center' }}>
        <p style={{ fontSize: '0.9rem', color: '#666' }}>Streaming inventory database arrays from service architecture...</p>
      </div>
    );
  }

  if (uiStatus.error) {
    return (
      <div className="auth-page-container" style={{ justifyContent: 'center' }}>
        <div className="admin-status-alert error" style={{ maxWidth: '400px', width: '100%', textAlign: 'center' }}>
          <strong>Service Status Intercept:</strong> <br />
          {uiStatus.error}
        </div>
      </div>
    );
  }

  return (
    <div className="main-content-fluid" style={{ padding: '30px 20px' }}>
      <div style={{ maxWidth: '1460px', margin: '0 auto' }}>

        {/* Dashboard Catalog Monitor Title */}
        <div style={{ borderBottom: '1px solid #ddd', paddingBottom: '10px', marginBottom: '25px' }}>
          <h1 style={{ fontSize: '1.7rem', fontWeight: '400', margin: 0, color: '#0f1111' }}>
            Live Inventory Catalog
          </h1>
          <p style={{ fontSize: '0.8rem', color: '#565959', margin: '4px 0 0 0' }}>
            Displaying {products.length} live database records verified via service provider pipeline layer.
          </p>
        </div>

        {/* Empty Catalog Fallback View */}
        {products.length === 0 && (
          <div style={{ textAlign: 'center', padding: '60px 20px', background: '#fff', border: '1px solid #d5d9d9', borderRadius: '8px' }}>
            <h3 style={{ fontWeight: '500', color: '#0f1111' }}>Your Inventory is Empty</h3>
            <p style={{ fontSize: '0.85rem', color: '#565959' }}>Head over to the Add Product admin hub to publish your first item metadata row.</p>
          </div>
        )}

        {/* 🏪 High-Fidelity Catalog Matrix Display Grid */}
        <div style={{
          display: 'grid',
          gridTemplateColumns: 'repeat(auto-fill, minmax(280px, 1fr))',
          gap: '20px'
        }}>
          {products
            .filter(item => !item.isDeleted)
            .map((item) => {

              const imageSource = item.imageUrl
                ? (item.imageUrl.startsWith('http') ? item.imageUrl : `https://localhost:7130${item.imageUrl}`)
                : 'https://placehold.co/300?text=No+Image';

              const displaySubCategory = item.subCategory && item.subCategory.includes('_')
                ? item.subCategory.split('_')[1]
                : item.subCategory;

              const isItemBusy = actionLoading[item.id] || false;

              return (
                <div key={item.id} className="product-card-container">

                  <div>
                    {/* Product Image Frame */}
                    <div className="single-image-wrapper" style={{ height: '220px', background: '#f7f7f7', borderRadius: '4px', overflow: 'hidden', marginBottom: '12px', width: '100%' }}>
                      <img
                        src={imageSource}
                        alt={item.name}
                        className="single-card-img"
                        onError={(e) => {
                          e.target.onerror = null;
                          e.target.src = 'https://placehold.co/300?text=Image+Load+Error';
                        }}
                      />
                    </div>

                    {/* Stock Status Badge Overlay */}
                    <div style={{ marginBottom: '8px' }}>
                      {item.inStock ? (
                        <span style={{ fontSize: '0.7rem', color: '#007600', background: '#e6f4ea', padding: '3px 8px', borderRadius: '12px', fontWeight: '700' }}>
                          In Stock ({item.stock})
                        </span>
                      ) : (
                        <span style={{ fontSize: '0.7rem', color: '#b12704', background: '#fce8e6', padding: '3px 8px', borderRadius: '12px', fontWeight: '700' }}>
                          Out of Stock
                        </span>
                      )}
                    </div>

                    {/* Info Text Layout Blocks */}
                    <h2 className="card-title" style={{ height: '40px', overflow: 'hidden', display: '-webkit-box', WebkitLineClamp: 2, WebkitBoxOrient: 'vertical' }}>
                      {item.name}
                    </h2>

                    {/* Categories & SubCategories */}
                    <p style={{ fontSize: '0.75rem', color: '#565959', margin: '0 0 10px 0' }}>
                      Category: <strong style={{ color: '#0f1111' }}>{item.category}</strong>
                      {item.subCategory && (
                        <span> | Sub: <strong style={{ color: '#0f1111' }}>{displaySubCategory}</strong></span>
                      )}
                    </p>

                    <p style={{
                      fontSize: '0.8rem',
                      color: '#333',
                      margin: '0 0 15px 0',
                      height: '50px',
                      overflow: 'hidden',
                      display: '-webkit-box',
                      WebkitLineClamp: 3,
                      WebkitBoxOrient: 'vertical',
                      lineHeight: '1.4'
                    }}>
                      {item.description || 'No product details provided.'}
                    </p>
                  </div>

                  {/* Pricing and Action Operational Control Center Wrapper */}
                  <div style={{ marginTop: 'auto' }}>
                    {/* Pricing Display */}
                    <div style={{ display: 'flex', alignItems: 'baseline', gap: '4px', borderTop: '1px solid #f3f3f3', paddingTop: '10px', marginBottom: '12px' }}>
                      <span style={{ fontSize: '0.75rem', color: '#0f1111', fontWeight: '500' }}>₹</span>
                      <span style={{ fontSize: '1.4rem', fontWeight: '700', color: '#0f1111' }}>
                        {Intl.NumberFormat('en-IN').format(item.price)}
                      </span>
                    </div>

                    {/* Action Button Segment Layer */}
                    <div style={{ display: 'flex', flexDirection: 'column', gap: '8px' }}>

                      {/* Button A: Add to Cart */}
                      <button
                        onClick={() => handleAddToCart(item)}
                        disabled={!item.inStock || isItemBusy}
                        style={{
                          border: '1px solid #a88734',
                          background: item.inStock ? 'linear-gradient(to bottom, #f7dfa5, #f0c14b)' : '#e7e9ec',
                          color: item.inStock ? '#111' : '#a2a6ac',
                          boxShadow: '0 1px 0 rgba(255,255,255,.4) inset'
                        }}
                      >
                        {isItemBusy ? 'Syncing...' : 'Add to Cart'}
                      </button>

                      {/* Button B: Buy Now */}
                      <button
                        onClick={() => handleBuyNow(item)}
                        disabled={!item.inStock || isItemBusy}
                        style={{
                          border: '1px solid #a88734',
                          background: item.inStock ? 'linear-gradient(to bottom, #f5b74a, #e69a10)' : '#eff1f3',
                          color: item.inStock ? '#111' : '#c8cbcc'
                        }}
                      >
                        Buy Now
                      </button>

                    </div>
                  </div>

                </div>
              );
            })}
        </div>
      </div>
    </div>
  );
};

export default Product;