import React from 'react'
import { useNavigate } from 'react-router-dom';

const ProductRow = (props) => {

    const row = props.row;
    const navigate = useNavigate();

  return (
    
      <div className="wide-deals-strip-container">
        <h2 className="strip-section-title">Today's Deals | Handpicked Top Offers</h2>
        <div className="horizontal-scroll-row">
          {Array.isArray(row) && row.map(item => {
            return (<div className="deal-thumb-box">
              <img src={item.imageUrl} alt={item.name} onClick={function(){navigate(`../product/${item.id}`)}} />
              <span className="deal-badge">Up to 35% Off</span>
              <p className="deal-desc">{item.name}</p>
            </div>)
          })}
        </div>
      </div>
  )
}

export default ProductRow