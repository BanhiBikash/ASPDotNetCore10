import React, { useState, useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axiosConfig';
import UserContext from '../context/UserContext'; 
import { useCart } from '../context/CartContext'; // 🎯 Added: Import your centralized custom hook 

const Product = () => {
  const { user } = useContext(UserContext); 
  const { cart, setCart } = useCart(); // 🎯 Added: Connect component to the global state tracker
  const navigate = useNavigate();
  const [products, setProducts] = useState([]);
  const [uiStatus, setUiStatus] = useState({ loading: true, error: null });
  const [actionLoading, setActionLoading] = useState({}); 

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

  // 🛒 Handle Add to Cart Strategy (Updates global state layout instantly)
  const handleAddToCart = async (product, silent = false) => {
    const productId = product.id;
    setActionLoading(prev => ({ ...prev, [productId]: true }));

    // 1️⃣ Create a copy of your existing state items array to alter safely
    let updatedItemsArray = [...cart.cart];
    
    // Check if the item already exists using a safe dual-schema fallback lookup
    const existingItemIndex = updatedItemsArray.findIndex(item => 
      item.productId === productId || (item.product && item.product.id === productId)
    );

    if (existingItemIndex !== -1) {
      updatedItemsArray[existingItemIndex].quantity += 1;
    } else {
      // Append a dual-schema object block matching both guest and backend DTO layouts
      updatedItemsArray.push({
        productId: product.id,
        quantity: 1,
        name: product.name,
        price: product.price,
        imageUrl: product.imageUrl,
        product: {
          id: product.id,
          name: product.name,
          price: product.price,
          imageUrl: product.imageUrl
        }
      });
    }

    // 🔐 CASE A: User is logged in -> Stream directly to clean token endpoint
    if (user && user.email) {
      try {
        // Target your parameterless backend endpoint. Token interceptor automatically signs it!
        const payload = {
          productId: productId,
          quantity: existingItemIndex !== -1 ? updatedItemsArray[existingItemIndex].quantity : 1
        };

        await api.post('/v1/Cart/UpdateCart', payload);

        // Commit directly to global state so Navbar updates instantly
        setCart({ cart: updatedItemsArray, isBusy: false });

        if (!silent) {
          alert(`Successfully added "${product.name}" to your account cart!`);
        }
      } catch (err) {
        console.error('Cart operation failure context:', err);
        alert(err.response?.data || 'Failed to update shopping cart allocation.');
      } finally {
        setActionLoading(prev => ({ ...prev, [productId]: false }));
      }
    }
    // 👥 CASE B: User is not logged in -> Fallback to LocalStorage Guest Cart
    else {
      try {
        // Commit state updates back onto browser cache parameter strings
        localStorage.setItem('guest_cart', JSON.stringify(updatedItemsArray));

        // Commit to context state instantly
        setCart({ cart: updatedItemsArray, isBusy: false });

        if (!silent) {
          alert(`"${product.name}" added to guest cart!`);
        }
      } catch (err) {
        console.error('Local storage cart operation exception context:', err);
        alert('Failed to update local guest cart matrix space.');
      } finally {
        setActionLoading(prev => ({ ...prev, [productId]: false }));
      }
    }
  };

  // ⚡ Handle Buy Now (Direct express checkout navigation without modifying the cart)
  const handleBuyNow = (product) => {
    if (!user || !user.email) {
      alert('Authentication required. Please log in to complete an express purchase.');
      navigate('/login');
      return;
    }
    navigate(`/Checkout/${product.id}`, { state: { directPurchaseItem: product } });
  };

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

        <div style={{ borderBottom: '1px solid #ddd', paddingBottom: '10px', marginBottom: '25px' }}>
          <h1 style={{ fontSize: '1.7rem', fontWeight: '400', margin: 0, color: '#0f1111' }}>
            Live Inventory Catalog
          </h1>
          <p style={{ fontSize: '0.8rem', color: '#565959', margin: '4px 0 0 0' }}>
            Displaying {products.length} live database records verified via service provider pipeline layer.
          </p>
        </div>

        {products.length === 0 && (
          <div style={{ textAlign: 'center', padding: '60px 20px', background: '#fff', border: '1px solid #d5d9d9', borderRadius: '8px' }}>
            <h3 style={{ fontWeight: '500', color: '#0f1111' }}>Your Inventory is Empty</h3>
            <p style={{ fontSize: '0.85rem', color: '#565959' }}>Head over to the Add Product admin hub to publish your first item metadata row.</p>
          </div>
        )}

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

                    <h2 className="card-title" style={{ height: '40px', overflow: 'hidden', display: '-webkit-box', WebkitLineClamp: 2, WebkitBoxOrient: 'vertical' }}>
                      {item.name}
                    </h2>

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

                  <div style={{ marginTop: 'auto' }}>
                    <div style={{ display: 'flex', alignItems: 'baseline', gap: '4px', borderTop: '1px solid #f3f3f3', paddingTop: '10px', marginBottom: '12px' }}>
                      <span style={{ fontSize: '0.75rem', color: '#0f1111', fontWeight: '500' }}>₹</span>
                      <span style={{ fontSize: '1.4rem', fontWeight: '700', color: '#0f1111' }}>
                        {Intl.NumberFormat('en-IN').format(item.price)}
                      </span>
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column', gap: '8px' }}>
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