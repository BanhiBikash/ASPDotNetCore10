import React from 'react';
import { useLocation, Link, useNavigate } from 'react-router-dom';

const Order = () => {
  const location = useLocation();
  const navigate = useNavigate();
  
  // Safely fetch state with optional chaining to prevent crashes on page refresh
  const orderData = location.state?.orderData;

  if (!orderData) {
    return (
      <div className="orders-error-panel-boundary">
        <div className="error-alert-box">
          <h5>No Order Data Found</h5>
          <p>We couldn't retrieve the details for this order. Please return to your order history tab.</p>
          <Link to="/orders" className="action-btn-pill-small" style={{ marginTop: '12px', display: 'inline-block' }}>
            ← Back to Your Orders
          </Link>
        </div>
      </div>
    );
  }

  // Helper function for status badges matching your OrderStatus options
  const getStatusBadgeClass = (status) => {
    switch (status?.toLowerCase()) {
      case 'pending':
      case 'processing': return 'status-badge-blue';
      case 'shipped': return 'status-badge-amber';
      case 'delivered': return 'status-badge-green';
      case 'cancelled':
      case 'failed': return 'status-badge-red';
      default: return 'status-badge-gray';
    }
  };

  // Helper function to format timestamps safely
  const formatDate = (dateString) => {
    if (!dateString) return "N/A";
    const options = { year: 'numeric', month: 'long', day: 'numeric', hour: '2-digit', minute: '2-digit' };
    return new Date(dateString).toLocaleDateString(undefined, options);
  };

  // 🎯 Check if the status allows address updates (Only true for Pending or Processing)
  const currentStatus = orderData.orderStatus?.toLowerCase();
  const isEditable = currentStatus === 'pending' || currentStatus === 'processing';

  // Handler for updating the address redirect layout flow
  const handleUpdateAddressClick = () => {
    console.log("Navigating to address edit form for Order:", orderData.id);
    // Redirect to your edit configuration view passing down the existing order state details
    navigate(`/account/update-address?orderId=${orderData.id}`, { state: { orderData } });
  };

  return (
    <div className="single-order-detail-fluid-container">
      {/* Breadcrumb navigation links */}
      <nav className="orders-breadcrumb-trail">
        <Link to="/account">Your Account</Link> › <Link to="/orders">Your Orders</Link> › <span className="active-trail">Order Details</span>
      </nav>

      <div className="single-order-meta-title-row">
        <h1>Order Details</h1>
        <p className="meta-subtitle-details">
          Ordered on {formatDate(orderData.ordeDate || orderData.orderDate)} <span className="divider-bar">|</span> Order ID: <span className="mono-id">{orderData.id}</span>
        </p>
      </div>

      {/* Grid Summary Panel: Shipping, Payment, and Summary snapshots */}
      <div className="order-summary-top-card-grid">
        
        {/* Box 1: Shipping Physical Destination + Conditional Action Button */}
        <div className="summary-grid-card unique-flex-layout-card">
          <div className="card-top-content-area">
            <h3>Shipping Address</h3>
            <div className="card-inner-address-text">
              <p className="shipping-user-name">Fulfillment Delivery</p>
              <p>{orderData.shippingAddress}</p>
              <p>{orderData.city}, {orderData.postalCode}</p>
              <p>{orderData.country || "India"}</p>
            </div>
          </div>
          
          {/* 🎯 The Live Update Address Button */}
          <div className="card-bottom-action-tray">
            <button 
              className={`amazon-address-update-btn ${!isEditable ? 'btn-disabled-state' : ''}`}
              disabled={!isEditable}
              onClick={handleUpdateAddressClick}
              title={isEditable ? "Change delivery destination particulars" : "Addresses cannot be modified once an order has left processing status"}
            >
              Update Address
            </button>
          </div>
        </div>

        {/* Box 2: Payment Execution Event Tracking */}
        <div className="summary-grid-card">
          <h3>Payment Method</h3>
          <div className="card-inner-address-text">
            <p className="payment-method-row">
              <span className="bullet-dot">✓</span> Digital Electronic Transaction Verified
            </p>
            <p className="payment-status-badge-label">
              Status: <span className={`status-pill-indicator ${getStatusBadgeClass(orderData.orderStatus)}`}>{orderData.orderStatus}</span>
            </p>
          </div>
        </div>

        {/* Box 3: Cost Accounting Invoice Balance Summary */}
        <div className="summary-grid-card cost-calculation-summary-panel">
          <h3>Order Summary</h3>
          <div className="invoice-rows-wrapper">
            <div className="invoice-row">
              <span>Items Subtotal:</span>
              <span>₹{orderData.totalAmount}</span>
            </div>
            <div className="invoice-row">
              <span>Shipping &amp; Handling:</span>
              <span className="green-free">₹0 (FREE)</span>
            </div>
            <hr className="invoice-split-line" />
            <div className="invoice-row total-row-highlight">
              <span>Grand Total:</span>
              <span>₹{orderData.totalAmount}</span>
            </div>
          </div>
        </div>

      </div>

      {/* Main Items Display Segment Card wrapper */}
      <div className="order-history-card-wrapper">
        <div className="order-card-panel-header single-order-view-header">
          <span className="items-count-indicator">
            Shipment Items ({orderData.items?.length || 0})
          </span>
        </div>
        
        <div className="order-card-panel-body">
          <div className="order-items-inner-collection">
            {orderData.items && orderData.items.map((item, index) => (
              <div key={`${orderData.id}-detail-item-${index}`} className="order-item-row-layout">
                <img 
                  src={item.imageUrl || "https://via.placeholder.com/100?text=No+Image"} 
                  alt={item.productName} 
                  className="order-item-thumbnail-pic" 
                />
                <div className="order-item-core-details">
                  <Link to={`/product/${item.productId}`} className="order-item-title-anchor">
                    {item.productName}
                  </Link>
                  <p className="order-item-meta-pricing-specs">
                    Quantity: <span className="dark-bold">{item.quantity}</span> 
                    <span className="divider-spacer">|</span> 
                    Unit Price: <span className="dark-bold">₹{item.unitPrice}</span>
                  </p>
                  <div className="order-item-action-links-row">
                    <Link to={`/product/${item.productId}`} className="continue-shopping-pill-btn special-buy-again-size">
                      Buy it again
                    </Link>
                  </div>
                </div>
                <div className="order-item-right-pricing-block">
                  <span className="item-calculated-subtotal">₹{item.unitPrice * item.quantity}</span>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default Order;