import React from 'react';

const ProductBox = ({ item, isItemBusy, handleAddToCart, handleBuyNow, baseUrl }) => {
  const imageSource = item.imageUrl || 'https://placehold.co/300?text=No+Image';
  
  const displaySubCategory = item.subCategory && item.subCategory.includes('_')
    ? item.subCategory.split('_')[1]
    : item.subCategory;

  return (
    <div className="search-result-row-card">
      
      {/* Left frame: Image Viewport */}
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

      {/* Right Frame: Specifications Details Panel */}
      <div className="row-card-details-frame">
        <div className="row-card-info-top">
          <h2 className="row-card-headline">{item.name}</h2>

          {/* Ratings Component Mock */}
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

          {/* Express Checkout Action Buttons Stack */}
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
};

export default ProductBox;