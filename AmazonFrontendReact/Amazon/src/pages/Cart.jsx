import React, { useState } from 'react';
import { useCart } from '../context/CartContext';
import { Link, useNavigate } from 'react-router-dom';
import api from '../api/axiosConfig'; // 🎯 Synced with your Context file path
import { baseUrl } from "../api/keyUrls"

const Cart = () => {
  // 🎯 FIXED: Destructure 'cart' and 'setCart' from context.
  // Note: 'cart' here represents the state object wrapper { cart: [], isBusy: false }
  const { cart: cartState, setCart } = useCart();
  const { cart: itemsArray, isBusy } = cartState;

  //for creating and storing cart
  const [order, setOrder] = useState({ items: [], ShippingAddress: null, PostalCode: null, City: null, Country: null });

  itemsArray.forEach(element => {
    console.log(element)
  });

  const navigate = useNavigate();

  // 🧮 Compute high-fidelity mathematical totals using the isolated array
  const totalItemsCount = itemsArray?.reduce((sum, item) => sum + (item.quantity || 0), 0) || 0;
  const totalCartPrice = itemsArray?.reduce((sum, item) => sum + ((item.price || 0) * (item.quantity || 0)), 0) || 0;

  /* ==========================================================================
     ⚙️ QUANTITY UPDATER MATRIX
     ========================================================================== */
  const handleQuantityChange = async (productId, currentQuantity, newQuantity) => {
    if (newQuantity < 1) return;

    // Optimistic UI Update matching your provider state blueprint
    const originalItemsArray = [...itemsArray];
    const updatedLocalItems = itemsArray.map(item =>
      item.productId === productId ? { ...item, quantity: newQuantity } : item
    );

    setCart({ cart: updatedLocalItems, isBusy: false });

    const token = localStorage.getItem('token');
    if (token) {
      try {
        await api.post('/v1/Cart/UpdateCart', {
          productId: productId,
          quantity: newQuantity
        });
      } catch (err) {
        console.error("Backend quantity sync failed. Rolling back changes:", err);
        setCart({ cart: originalItemsArray, isBusy: false });
      }
    } else {
      localStorage.setItem('guest_cart', JSON.stringify(updatedLocalItems));
    }
  };

  /* ==========================================================================
     🗑️ ITEM REMOVAL ROUTINE
     ========================================================================== */
  const handleRemoveItem = async (productId) => {
    const originalItemsArray = [...itemsArray];
    const updatedLocalItems = itemsArray.filter(item => item.productId !== productId);

    setCart({ cart: updatedLocalItems, isBusy: false });

    const token = localStorage.getItem('token');
    if (token) {
      try {
        await api.delete(`/v1/Cart/RemoveItem?productId=${productId}`);
      } catch (err) {
        console.error("Backend removal failed. Restoring cart context:", err);
        setCart({ cart: originalItemsArray, isBusy: false });
      }
    } else {
      localStorage.setItem('guest_cart', JSON.stringify(updatedLocalItems));
    }
  };

  const handleCheckoutNavigation = () => {

    //check if user is logged in then move to checkout otherwise send to login
    const token = localStorage.getItem('token');
    if (!token) {
      navigate('/login?redirect=checkout');
    }

    //live cart state to specific backend entity field names
    const formattedOrderItems = itemsArray.map(item => ({
      productId: item.productId,
      productName: item.name || item.productName || '',
      imageUrl: item.imageUrl || '',
      quantity: item.quantity || 0,
      unitPrice: item.price || item.unitPrice || 0
    }));

    //Packaged the items and pricing into a complete order prop structure
    const orderDataPayload = {
      items: formattedOrderItems,
      totalAmount: totalCartPrice,
      shippingAddress: "", // Initializing empty for the form inputs next page
      postalCode: "",
      city: "",
      country: ""
    };

    // 🚀 Navigate and attach the payload property into the browser's history state
    navigate('/checkout', { state: { orderData: orderDataPayload } });
  };

  if (isBusy) {
    return (
      <div className="cart-loading-spinner-box">
        <p>Loading your shopping basket details...</p>
      </div>
    );
  }

  return (
    <div className="cart-page-fluid-container">
      <div className="cart-main-layout-wrapper">

        {/* 📦 LEFT COLUMN: MAIN SHOPPING CART DETAILS LIST */}
        <div className="cart-items-collection-panel">
          <div className="cart-header-title-block">
            <h1>Shopping Cart</h1>
            {itemsArray && itemsArray.length > 0 && <span className="cart-price-header-label">Price</span>}
          </div>
          <hr className="cart-layout-divider" />

          {(!itemsArray || itemsArray.length === 0) ? (
            <div className="cart-empty-state-fallback">
              <h3>Your Shopping Cart is empty.</h3>
              <p>Check out today's deals or continue exploring our product catalog.</p>
              <Link to="/" className="amazon-primary-btn style-inline">Continue Shopping</Link>
            </div>
          ) : (
            itemsArray.map((item) => (
              <div key={item.productId} className="cart-item-row-node">
                <div className="cart-item-image-wrapper">
                  <img
                    src={item.imageUrl || 'https://via.placeholder.com/150?text=Product'}
                    alt={item.name || 'Catalog Product'}
                  />
                </div>

                <div className="cart-item-details-body">
                  <h2 className="cart-item-title-text">{item.name || "Amazon Verified Product"}</h2>
                  <p className="cart-item-stock-status">In Stock</p>
                  <p className="cart-item-shipping-promo">Eligible for FREE Shipping</p>

                  <div className="cart-item-actions-row">
                    <div className="cart-quantity-selector-container">
                      <button
                        type="button"
                        onClick={() => handleQuantityChange(item.productId, item.quantity, item.quantity - 1)}
                        disabled={item.quantity <= 1}
                      >
                        -
                      </button>
                      <span className="cart-quantity-display-value">{item.quantity}</span>
                      <button
                        type="button"
                        onClick={() => handleQuantityChange(item.productId, item.quantity, item.quantity + 1)}
                      >
                        +
                      </button>
                    </div>
                    <span className="cart-action-split-pipe">|</span>
                    <button
                      type="button"
                      className="cart-delete-trigger-btn"
                      onClick={() => handleRemoveItem(item.productId)}
                    >
                      Delete
                    </button>
                  </div>
                </div>

                <div className="cart-item-price-column">
                  <span className="cart-item-calculated-price">
                    ₹{(item.price || 0).toLocaleString('en-IN')}
                  </span>
                </div>
              </div>
            ))
          )}

          {itemsArray && itemsArray.length > 0 && (
            <div className="cart-subtotal-summary-row border-top-split">
              <h3>Subtotal ({totalItemsCount} item{totalItemsCount !== 1 ? 's' : ''}): <strong>₹{totalCartPrice.toLocaleString('en-IN')}</strong></h3>
            </div>
          )}
        </div>

        {/* 💳 RIGHT COLUMN: DOUBLE-CHECKOUT ACCESSIBILITY PANEL */}
        {itemsArray && itemsArray.length > 0 && (
          <div className="cart-checkout-sticky-panel">

            {/* 🎯 CONVENIENCE CHECKOUT MODULE 1: TOP POSITION */}
            <div className="checkout-widget-block boundary-bottom-split">
              <div className="checkout-subtotal-preview">
                <h2>Subtotal ({totalItemsCount} items): <br /><strong>₹{totalCartPrice.toLocaleString('en-IN')}</strong></h2>
              </div>
              <div className="checkout-free-shipping-indicator">
                <span className="checkmark-icon">✓</span> Your order qualifies for FREE Delivery.
              </div>
              <button
                type="button"
                className="amazon-primary-btn checkout-action-btn-w100"
                onClick={handleCheckoutNavigation}
              >
                Proceed to Checkout (Top)
              </button>
            </div>

            {/* 🎯 CONVENIENCE CHECKOUT MODULE 2: BOTTOM POSITION */}
            <div className="checkout-widget-block padding-top-spacing">
              <p className="checkout-urgency-notice">Items in your cart are not reserved. Secure your order before stocks fluctuate.</p>
              <button
                type="button"
                className="amazon-primary-btn checkout-action-btn-w100 alternate-color-btn"
                onClick={handleCheckoutNavigation}
              >
                Proceed to Checkout (Bottom)
              </button>
            </div>

          </div>
        )}

      </div>
    </div>
  );
};

export default Cart;