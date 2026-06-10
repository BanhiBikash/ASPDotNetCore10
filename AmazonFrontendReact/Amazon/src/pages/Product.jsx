import React, { useState, useEffect, useContext } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import api from '../api/axiosConfig';
import UserContext from '../context/UserContext';
import { useCart } from '../context/CartContext';
import { baseUrl, checkoutUrl } from '../api/keyUrls';
import ProductRow from '../Components/ProductRow';

//get icons
import cod from "../assets/cod.png"
import free_shipping from "../assets/icon_free_shipping.png"
import secure_pay from "../assets/secure_pay.png"
import top_brand from "../assets/top_brand.png"

const Product = () => {
  // 🎯 Extract id from the routing path (e.g., /product/:id)
  const { id } = useParams();
  const navigate = useNavigate();

  const { user } = useContext(UserContext);
  const { cart: cartData, setCart } = useCart();
  const { cart: itemsArray } = cartData;

  const [product, setProduct] = useState(null);
  const [relatedProducts, setRelatedProducts] = useState([]);
  const [uiStatus, setUiStatus] = useState({ loading: true, error: null });
  const [actionLoading, setActionLoading] = useState({});

  // 🎯 Track if the main viewed item is already present in your active cart state
  const existingCartItem = itemsArray?.find(cartItem =>
    cartItem.productId === id || (cartItem.product && cartItem.product.id === id)
  );

  // 📡 Step 1: Fetch Main Product Details and Chain Related SubCategory items
  useEffect(() => {
    const fetchProductDataAndRelated = async () => {
      if (!id) return;
      setUiStatus(prev => ({ ...prev, loading: true }));

      try {
        // 1. Fetch main targeted product profile
        const productResponse = await api.get(`/v1/Products/${id}`);
        const currentItem = productResponse.data;
        setProduct(currentItem);

        // 2. Chained Fetch: Get matching rows of the same subcategory enum item
        if (currentItem && currentItem.subCategory) {
          const relatedResponse = await api.get(`/v1/Products/subcategory/${encodeURIComponent(currentItem.subCategory)}`);
          const matchingList = Array.isArray(relatedResponse.data) ? relatedResponse.data : [];

          // Filter out the current active item so it doesn't recommend itself
          setRelatedProducts(matchingList.filter(item => item.id !== currentItem.id && !item.isDeleted));
        }

        setUiStatus({ loading: false, error: null });
      } catch (err) {
        console.error('Handshake inventory matching execution failure:', err);
        const backendErrorMessage = err.response?.data || err.message || 'Failed to sync product profile details.';
        setUiStatus({
          loading: false,
          error: typeof backendErrorMessage === 'string' ? backendErrorMessage : 'Database service context offline.'
        });
      }
    };

    fetchProductDataAndRelated();
  }, [id]);

  // 🛒 Handle Add to Cart / Increment Step
  const handleAddToCart = async (targetProduct, silent = false) => {
    const productId = targetProduct.id;
    setActionLoading(prev => ({ ...prev, [productId]: true }));

    let updatedItemsArray = [...itemsArray];
    const existingItemIndex = updatedItemsArray.findIndex(item =>
      item.productId === productId || (item.product && item.product.id === productId)
    );

    if (existingItemIndex !== -1) {
      updatedItemsArray[existingItemIndex].quantity += 1;
    } else {
      updatedItemsArray.push({
        productId: productId,
        quantity: 1,
        name: targetProduct.name,
        price: targetProduct.price,
        imageUrl: targetProduct.imageUrl,
        product: { ...targetProduct }
      });
    }

    try {
      if (user && user.email) {
        const payload = {
          productId: productId,
          quantity: existingItemIndex !== -1 ? updatedItemsArray[existingItemIndex].quantity : 1
        };
        await api.post('/v1/Cart/UpdateCart', payload);
      } else {
        localStorage.setItem('guest_cart', JSON.stringify(updatedItemsArray));
      }

      setCart({ cart: updatedItemsArray, isBusy: false });
      if (!silent) alert(`Successfully added "${targetProduct.name}" to cart!`);
    } catch (err) {
      console.error('Cart assignment error:', err);
      alert(err.response?.data || 'Failed to update shopping cart allocation.');
    } finally {
      setActionLoading(prev => ({ ...prev, [productId]: false }));
    }
  };

  // 🔄 Handle Decrementing Item Quantity
  const handleDecrementCart = async (targetProduct) => {
    const productId = targetProduct.id;
    if (!existingCartItem) return;

    setActionLoading(prev => ({ ...prev, [productId]: true }));
    let updatedItemsArray = [...itemsArray];
    const itemIndex = updatedItemsArray.findIndex(item =>
      item.productId === productId || (item.product && item.product.id === productId)
    );

    const nextQty = existingCartItem.quantity - 1;

    if (nextQty > 0) {
      updatedItemsArray[itemIndex].quantity = nextQty;
    } else {
      updatedItemsArray.splice(itemIndex, 1);
    }

    try {
      if (user && user.email) {
        const payload = { productId, quantity: nextQty };
        await api.post('/v1/Cart/UpdateCart', payload);
      } else {
        localStorage.setItem('guest_cart', JSON.stringify(updatedItemsArray));
      }

      setCart({ cart: updatedItemsArray, isBusy: false });
    } catch (err) {
      console.error('Failed syncing decrement operation:', err);
    } finally {
      setActionLoading(prev => ({ ...prev, [productId]: false }));
    }
  };

  const handleBuyNow = (targetProduct) => {
    if (!user || !user.email) {
      alert('Authentication required. Please log in to complete checkout.');
      navigate('/login');
      return;
    }
    navigate(`${checkoutUrl}/${targetProduct.id}`, { state: { directPurchaseItem: targetProduct } });
  };

  if (uiStatus.loading) {
    return (
      <div className="auth-page-container fallback-center">
        <p className="catalog-loading-text">Streaming product specifications matrix from service architecture...</p>
      </div>
    );
  }

  if (uiStatus.error || !product) {
    return (
      <div className="auth-page-container fallback-center">
        <div className="admin-status-alert error alert-constrained">
          <strong>Service Status Intercept:</strong> <br />
          {uiStatus.error || 'Product variant data unreadable.'}
        </div>
      </div>
    );
  }

  const imageSource = product.imageUrl || 'https://placehold.co/400?text=No+Image';
  const displaySubCategory = product.subCategory && product.subCategory.includes('_')
    ? product.subCategory.split('_')[1]
    : product.subCategory;

  return (
    <div className="main-content-fluid product-details-page-override">

      {/* 🛠️ UPPER SECTION: Main Focus Product Split Frame */}
      <div className="product-showcase-container" style={styles.showcaseFlex}>

        {/* Left Side: Massive Image Viewport Block */}
        <div className="product-image-hero-frame" style={styles.imageHeroBox}>
          <img
            src={imageSource}
            alt={product.name}
            style={styles.heroImg}
            onError={(e) => {
              e.target.onerror = null;
              e.target.src = 'https://placehold.co/400?text=Image+Load+Error';
            }}
          />
        </div>

        {/* Right Side: Identity, Specs, and Actions Panel */}
        <div className="product-specs-info-panel" style={styles.infoPanel}>
          <h1 className="product-main-title" style={styles.mainTitle}>{product.name}</h1>

          <div className="row-card-rating-line" style={{ marginBottom: '12px' }}>
            <span className="stars-gold">★★★★☆</span>
            <span className="rating-count-link" style={{ marginLeft: '8px' }}>2,410 customer reviews</span>
          </div>

          <hr style={styles.divider} />

          <p style={styles.metaRow}>
            Category: <strong style={{ textTransform: 'capitalize' }}>{product.category}</strong>
            {product.subCategory && (
              <span> | Subcategory: <strong style={{ textTransform: 'capitalize' }}>{displaySubCategory}</strong></span>
            )}
          </p>

          <div className="product-pricing-block" style={styles.priceContainer}>
            <span style={styles.currencySymbol}>₹</span>
            <span style={styles.priceDigits}>{Intl.NumberFormat('en-IN').format(product.price)}</span>

            <div style={{ marginTop: '8px' }}>
              {product.inStock ? (
                <span className="stock-indicator-badge in-stock">In Stock ({product.stock} items left)</span>
              ) : (
                <span className="stock-indicator-badge out-of-stock">Temporarily Out of Stock</span>
              )}
            </div>
          </div>

          <div className="product-description-container">
            <h4>Product Information</h4>
            <p style={styles.descBody}>{product.description || 'Detailed technical specs haven\'t been allocated for this model option.'}</p>
          </div>
        </div>

        {/* right-most section */}
        <div className="products-specs-purchase-panel">

          {/* 1. Precise Delivery Window */}
          <div className="checkout-delivery-promise-block">
            <span className="delivery-highlight-date">
              FREE delivery <span className="bold-text">Wednesday, June 17</span>
            </span>
            <span className="delivery-subtext">
              Or fastest delivery <span className="bold-text">Sunday, June 14</span>
              <br />
              Order within <span className="timer-green">14 hrs 32 mins</span>
            </span>
          </div>

          {/* 2. Geolocation / Shipping Target */}
          <div className="checkout-geo-location-anchor">
            <span className="geo-pin-icon">📍</span>
            <span className="geo-location-text">Deliver to India</span>
          </div>

          {/* 3. Transaction Meta Trust Details */}
          <div className="checkout-trust-meta-table">
            <div className="meta-table-row">
              <span className="meta-label">Ships from</span>
              <span className="meta-value link-style">Amazon.com</span>
            </div>
            <div className="meta-table-row">
              <span className="meta-label">Sold by</span>
              <span className="meta-value link-style">RetailerNet Ltd</span>
            </div>
          </div>

          {/* 4. Customer Control Options (Gift Flag) */}
          <div className="checkout-gift-checkbox-row">
            <input type="checkbox" id="isAGift" name="isAGift" />
            <label htmlFor="isAGift">Add a gift receipt for easy returns</label>
          </div>

          {/* Icons Row */}
          <div className="product_amazon_icon">
            <img className='product_promise' src={secure_pay} alt="secure-pay" />
            <img className='product_promise' src={free_shipping} alt="free_shipping" />
            <img className='product_promise' src={cod} alt="cod_icon" />
            <img className='product_promise' src={top_brand} alt="top_brand" />
          </div>

          {/* 🔘 ACTION ROW CONTROLS */}
          <div className="product-express-checkout-row" style={styles.actionButtonRow}>
            {existingCartItem ? (
              <div className="qtyPill-container">
                <button
                  onClick={() => handleDecrementCart(product)}
                  disabled={actionLoading[product.id]}
                  className="qtyPill-button"
                >
                  −
                </button>
                <span style={{ fontWeight: '600', color: '#0f1111' }}>{existingCartItem.quantity}</span>
                <button
                  onClick={() => handleAddToCart(product, true)}
                  disabled={!product.inStock || actionLoading[product.id] || existingCartItem.quantity >= product.stock}
                  className="qtyPill-button"
                >
                  +
                </button>
              </div>
            ) : (
              <button
                onClick={() => handleAddToCart(product)}
                disabled={!product.inStock || actionLoading[product.id]}
                className={`amazon-pill-btn cart ${!product.inStock ? 'disabled' : ''}`}
                style={{ minWidth: '150px' }}
              >
                {actionLoading[product.id] ? 'Updating...' : 'Add to Cart'}
              </button>
            )}

            <button
              onClick={() => handleBuyNow(product)}
              disabled={!product.inStock || actionLoading[product.id]}
              className={`amazon-pill-btn buy-now ${!product.inStock ? 'disabled' : ''}`}
              style={{ minWidth: '150px' }}
            >
              Buy Now
            </button>
          </div>
        </div>

      </div>

      {/* 🛠️ LOWER SECTION: Same SubCategory Recommendations Carousel Track */}
      <div className="related-cross-sell-shelf" style={styles.relatedShelf}>
        <h3 style={styles.shelfTitle}>Customers Who Bought Items In "{displaySubCategory}" Also Viewed</h3>

        {relatedProducts.length === 0 ? (
          <p style={{ color: '#666', fontStyle: 'italic', padding: '10px 0' }}>No complementary items available in this category cluster yet.</p>
        ) : (
          <div className="related-items-horizontal-track" style={styles.horizontalScrollTrack}>
            {relatedProducts.map((item) => (
              <div
                key={item.id}
                style={styles.suggestionCard}
                onClick={() => navigate(`/product/${item.id}`)} // Route jumping updates view smoothly
              >
                <div style={styles.suggestImgWrap}>
                  <img
                    src={item.imageUrl || 'https://placehold.co/150?text=No+Image'}
                    alt={item.name}
                    style={styles.suggestImg}
                  />
                </div>
                <h4 style={styles.suggestName}>{item.name}</h4>
                <p style={styles.suggestPrice}>₹{Intl.NumberFormat('en-IN').format(item.price)}</p>
              </div>
            ))}
          </div>
        )}
      </div>

    </div>
  );
};

// Layout Object Configuration
const styles = {
  showcaseFlex: {
    display: 'flex',
    flexWrap: 'wrap',
    gap: '40px',
    padding: '20px 0',
    alignItems: 'flex-start'
  },
  imageHeroBox: {
    flex: '1 1 400px',
    maxWidth: '500px',
    border: '1px solid #eeeeee',
    borderRadius: '8px',
    backgroundColor: '#ffffff',
    padding: '20px',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center'
  },
  heroImg: {
    maxWidth: '100%',
    maxHeight: '450px',
    objectFit: 'contain'
  },
  infoPanel: {
    flex: '1 1 500px',
    padding: '0 10px'
  },
  mainTitle: {
    fontSize: '2rem',
    fontWeight: '500',
    color: '#0f1111',
    margin: '0 0 10px 0',
    lineHeight: '1.2'
  },
  divider: {
    border: '0',
    height: '1px',
    backgroundColor: '#e7e7e7',
    margin: '15px 0'
  },
  metaRow: {
    fontSize: '0.95rem',
    color: '#565959',
    margin: '0 0 15px 0'
  },
  priceContainer: {
    backgroundColor: '#fafafa',
    padding: '15px',
    borderRadius: '4px',
    marginBottom: '20px'
  },
  currencySymbol: {
    fontSize: '1.2rem',
    verticalAlign: 'super',
    marginRight: '2px',
    color: '#0f1111'
  },
  priceDigits: {
    fontSize: '2.2rem',
    fontWeight: '400',
    color: '#0f1111'
  },
  descBody: {
    fontSize: '1rem',
    lineHeight: '1.6',
    color: '#333333',
    margin: '8px 0 20px 0'
  },
  actionButtonRow: {
    display: 'flex',
    alignItems: 'center',
    gap: '15px',
    marginTop: '25px',
    flexWrap: 'wrap'
  },
  qtyPill: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'space-evenly',
    backgroundColor: '#ffd814',
    border: '1px solid #fcd200',
    borderRadius: '20px',
    width: '150px',
    height: '35px'
  },
  qtyBtn: {
    background: 'transparent',
    border: 'none',
    fontSize: '1.2rem',
    fontWeight: 'bold',
    cursor: 'pointer',
    padding: '0 15px',
    height: '100%'
  },
  relatedShelf: {
    marginTop: '60px',
    borderTop: '2px solid #eaeded',
    paddingTop: '30px',
    width: '100%'
  },
  shelfTitle: {
    fontSize: '1.35rem',
    fontWeight: '700',
    color: '#cc6600',
    marginBottom: '20px'
  },
  horizontalScrollTrack: {
    display: 'flex',
    gap: '20px',
    overflowX: 'auto',
    paddingBottom: '15px',
    scrollbarWidth: 'thin'
  },
  suggestionCard: {
    flex: '0 0 180px',
    border: '1px solid #e7e7e7',
    borderRadius: '6px',
    padding: '12px',
    backgroundColor: '#fff',
    cursor: 'pointer',
    transition: 'transform 0.15s ease',
    textAlign: 'center'
  },
  suggestImgWrap: {
    height: '140px',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    marginBottom: '10px',
    backgroundColor: '#fff'
  },
  suggestImg: {
    maxHeight: '100%',
    maxWidth: '100%',
    objectFit: 'contain'
  },
  suggestName: {
    fontSize: '0.9rem',
    fontWeight: '500',
    color: '#007185',
    margin: '0 0 6px 0',
    height: '36px',
    overflow: 'hidden',
    display: '-webkit-box',
    WebkitLineClamp: 2,
    WebkitBoxOrient: 'vertical'
  },
  suggestPrice: {
    fontSize: '1rem',
    fontWeight: '600',
    color: '#b12704',
    margin: 0
  }, product_amazon_icon: {
    display: 'flex',
    justifyContent: 'space-evenly',
    alignItems: 'center'
  },
  product_promise: {
    width: '4px',
    height: '4px'
  }
};

export default Product;