import React, { useState, useEffect, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import api from '../api/axiosConfig';
import UserContext from '../context/UserContext'; 
import { useCart } from '../context/CartContext'; 
import { baseUrl } from '../api/keyUrls';

const Product = () => {
  const { user } = useContext(UserContext); 
  const { cart, setCart } = useCart(); 
  const navigate = useNavigate();
  const [products, setProducts] = useState([]);
  const [uiStatus, setUiStatus] = useState({ loading: true, error: null });
  const [actionLoading, setActionLoading] = useState({}); 

  // 🎛️ Filter and Sort States (Bound directly to backend API layer)
  const [sortBy, setSortBy] = useState('');
  const [maxPrice, setMaxPrice] = useState('');
  const [minRating, setMinRating] = useState('');

  // 📡 Fetch catalog array on component mount or when filters change
  useEffect(() => {
    const fetchCatalog = async () => {
      setUiStatus(prev => ({ ...prev, loading: true }));
      try {
        const params = {};
        if (sortBy) params.sortBy = sortBy;
        if (maxPrice) params.maxPrice = maxPrice;
        if (minRating) params.minRating = minRating;

        const response = await api.get('/v1/Products', { params });
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
  }, [sortBy, maxPrice, minRating]);

  // 🛒 Handle Add to Cart Strategy
  const handleAddToCart = async (product, silent = false) => {
    const productId = product.id;
    setActionLoading(prev => ({ ...prev, [productId]: true }));

    let updatedItemsArray = [...cart.cart];
    
    const existingItemIndex = updatedItemsArray.findIndex(item => 
      item.productId === productId || (item.product && item.product.id === productId)
    );

    if (existingItemIndex !== -1) {
      updatedItemsArray[existingItemIndex].quantity += 1;
    } else {
      updatedItemsArray.push({
        productId: product.id,
        quantity: 1,
        name: product.name,
        price: product.price,
        imageUrl: baseUrl + product.imageUrl,
        product: {
          id: product.id,
          name: product.name,
          price: product.price,
          imageUrl: product.imageUrl
        }
      });
    }

    if (user && user.email) {
      try {
        const payload = {
          productId: productId,
          quantity: existingItemIndex !== -1 ? updatedItemsArray[existingItemIndex].quantity : 1
        };

        await api.post('/v1/Cart/UpdateCart', payload);
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
    } else {
      try {
        localStorage.setItem('guest_cart', JSON.stringify(updatedItemsArray));
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

  const handleBuyNow = (product) => {
    if (!user || !user.email) {
      alert('Authentication required. Please log in to complete an express purchase.');
      navigate('/login');
      return;
    }
    navigate(`/Checkout/${product.id}`, { state: { directPurchaseItem: product } });
  };

  const clearFilters = () => {
    setSortBy('');
    setMaxPrice('');
    setMinRating('');
  };

  if (uiStatus.loading && products.length === 0) {
    return (
      <div className="auth-page-container fallback-center">
        <p className="catalog-loading-text">Streaming inventory database arrays from service architecture...</p>
      </div>
    );
  }

  if (uiStatus.error) {
    return (
      <div className="auth-page-container fallback-center">
        <div className="admin-status-alert error alert-constrained">
          <strong>Service Status Intercept:</strong> <br />
          {uiStatus.error}
        </div>
      </div>
    );
  }

  return (
  <div className="main-content-fluid catalog-root-override">
    
    {/* Top Banner Meta Bar */}
    <div className="catalog-meta-banner">
      <p>
        Displaying <span>{products.length} results</span> verified via service provider pipeline layer.
      </p>
    </div>

    {/* Main Layout Scaffolding */}
    <div className="catalog-scaffolding">
      
      {/* 🛠️ LEFT SIDEBAR: Transforms cleanly on mobile viewports */}
      <aside className="catalog-filter-sidebar">
        <div className="sidebar-filter-header">
          <h3>Filters</h3>
          <button onClick={clearFilters} className="clear-filters-btn">Clear all</button>
        </div>

        <div className="sidebar-filter-groups-wrapper">
          {/* Sort Block */}
          <div className="filter-group-block">
            <h4>Sort By</h4>
            <select value={sortBy} onChange={(e) => setSortBy(e.target.value)} className="sidebar-select-input">
              <option value="">Featured</option>
              <option value="price_asc">Price: Low to High</option>
              <option value="price_desc">Price: High to Low</option>
              <option value="rating_desc">Avg. Customer Review</option>
            </select>
          </div>

          {/* Price Radio Block */}
          <div className="filter-group-block">
            <h4>Price Budget</h4>
            <div className="filter-radio-stack">
              <label>
                <input type="radio" name="price" checked={maxPrice === ''} onChange={() => setMaxPrice('')} /> Any Price
              </label>
              <label>
                <input type="radio" name="price" checked={maxPrice === '1000'} onChange={() => setMaxPrice('1000')} /> Under ₹1,000
              </label>
              <label>
                <input type="radio" name="price" checked={maxPrice === '5000'} onChange={() => setMaxPrice('5000')} /> Under ₹5,000
              </label>
              <label>
                <input type="radio" name="price" checked={maxPrice === '10000'} onChange={() => setMaxPrice('10000')} /> Under ₹10,000
              </label>
            </div>
          </div>

          {/* Star Rating Block */}
          <div className="filter-group-block">
            <h4>Customer Review</h4>
            <div className="filter-radio-stack">
              {[4, 3, 2, 1].map((stars) => (
                <button
                  key={stars}
                  onClick={() => setMinRating(stars.toString())}
                  className={`rating-star-filter-link ${minRating === stars.toString() ? 'active' : ''}`}
                >
                  {'★'.repeat(stars)}{'☆'.repeat(5 - stars)} & Up
                </button>
              ))}
            </div>
          </div>
        </div>
      </aside>

      {/* 📋 RIGHT CONTENT FIELD: Product Search Listings Stream */}
      <main className="catalog-results-viewspace">
        {products.length === 0 && (
          <div className="catalog-empty-container">
            <h3>No Results Match Filter Parameters</h3>
            <p>Try clearing your active sidebar choices to restore items streaming.</p>
          </div>
        )}

        <div className="catalog-vertical-stack">
          {products
            .filter(item => !item.isDeleted)
            .map((item) => {
              const imageSource = item.imageUrl || 'https://placehold.co/300?text=No+Image';
              const displaySubCategory = item.subCategory && item.subCategory.includes('_')
                ? item.subCategory.split('_')[1]
                : item.subCategory;
              const isItemBusy = actionLoading[item.id] || false;

              return (
                <div key={item.id} className="search-result-row-card">
                  
                  {/* Left frame: Image Frame Container */}
                  <div className="row-card-image-viewport">
                    <img
                      src={imageSource}
                      alt={item.name}
                      onError={(e) => {
                        e.target.onerror = null;
                        e.target.src = 'https://placehold.co/300?text=Image+Load+Error';
                      }}
                    />
                  </div>

                  {/* Right Frame: Specifications Content Frame */}
                  <div className="row-card-details-frame">
                    <div className="row-card-info-top">
                      <h2 className="row-card-headline">{item.name}</h2>

                      {/* Ratings */}
                      <div className="row-card-rating-line">
                        <span className="stars-gold">★★★★☆</span>
                        <span className="rating-count-link">2,410 ratings</span>
                      </div>

                      <p className="row-card-category-meta">
                        Category: <strong>{item.category}</strong>
                        {item.subCategory && (
                          <span> | Sub: <strong>{displaySubCategory}</strong></span>
                        )}
                      </p>

                      <p className="row-card-description-body">
                        {item.description || 'No product details provided.'}
                      </p>
                    </div>

                    {/* Pricing block and actions panel wrapper */}
                    <div className="row-card-footer-action-panel">
                      <div className="price-tag-container">
                        <div className="price-tag-digits">
                          <span className="currency-symbol">₹</span>
                          <span className="amount-number">
                            {Intl.NumberFormat('en-IN').format(item.price)}
                          </span>
                        </div>
                        <div className="stock-indicator-height">
                          {item.inStock ? (
                            <span className="stock-indicator-badge in-stock">
                              In Stock ({item.stock} units)
                            </span>
                          ) : (
                            <span className="stock-indicator-badge out-of-stock">
                              Out of Stock
                            </span>
                          )}
                        </div>
                      </div>

                      {/* Action buttons stack */}
                      <div className="row-card-buttons-group">
                        <button
                          onClick={() => handleAddToCart(item)}
                          disabled={!item.inStock || isItemBusy}
                          className={`amazon-pill-btn cart ${!item.inStock ? 'disabled' : ''}`}
                        >
                          {isItemBusy ? 'Syncing...' : 'Add to Cart'}
                        </button>

                        <button
                          onClick={() => handleBuyNow(item)}
                          disabled={!item.inStock || isItemBusy}
                          className={`amazon-pill-btn buy-now ${!item.inStock ? 'disabled' : ''}`}
                        >
                          Buy Now
                        </button>
                      </div>
                    </div>

                  </div>
                </div>
              );
            })}
        </div>
      </main>
    </div>
  </div>
);
};

export default Product;