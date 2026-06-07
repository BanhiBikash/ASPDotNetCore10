import React from 'react'
import { useLocation } from 'react-router-dom'
const Order = () => {

    const location = useLocation();
    //fetch data
    const orderData = location.state.orderData;
if (!orderData) {
    return <div>No order data found. Please return to the history tab.</div>;
  }

  return (
    <div className="order-detail-container">
      <h2>Reviewing Order Details</h2>
      <p><strong>ID:</strong> {orderData.id}</p>
      <p><strong>Status:</strong> {orderData.orderStatus}</p>
      <p><strong>City Destination:</strong> {orderData.shippingAddress}</p>
    </div>
  );
}

export default Order