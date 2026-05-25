import React, { useState, useEffect } from 'react';
import api from '../api/axiosConfig';

const Product = () => {
  const [products, setProducts] = useState([]);
  const [uiStatus, setUiStatus] = useState({ loading: true, error: null });

  // 📡 Fetch catalog array on component mount
  useEffect(() => {
    const fetchCatalog = async () => {
      try {
        const response = await api.get('/v1/Products');
        
        // Maps perfectly to your direct JSON array return pipeline
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
            .filter(item => !item.isDeleted) // Filters out any soft-deleted items automatically
            .map((item) => {
              
              // 📸 Matches your DTO's camelCased item.imageUrl string property handles
              const imageSource = item.imageUrl
                ? (item.imageUrl.startsWith('http') ? item.imageUrl : `https://localhost:7130${item.imageUrl}`)
                : 'https://placehold.co/300?text=No+Image';

              // Formats camelCase subcategory formatting safely: "HomeAppliance_Kitchen" -> "Kitchen"
              const displaySubCategory = item.subCategory && item.subCategory.includes('_')
                ? item.subCategory.split('_')[1]
                : item.subCategory;

              return (
                <div key={item.id} style={{
                  background: '#ffffff',
                  border: '1px solid #e7e7e7',
                  borderRadius: '8px',
                  padding: '15px',
                  display: 'flex',
                  flexDirection: 'column',
                  justifyContent: 'between',
                  boxShadow: '0 1px 3px rgba(0,0,0,0.05)',
                  position: 'relative'
                }}>
                  
                  {/* Product Image Frame */}
                  <div style={{ 
                    width: '100%', 
                    height: '220px', 
                    display: 'flex', 
                    alignItems: 'center', 
                    justifyContent: 'center',
                    background: '#f7f7f7',
                    borderRadius: '4px',
                    overflow: 'hidden',
                    marginBottom: '12px'
                  }}>
                    <img 
                      src={imageSource} 
                      alt={item.name} 
                      style={{ maxWidth: '100%', maxHeight: '100%', objectFit: 'contain' }}
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
                  <h2 style={{ fontSize: '1rem', fontWeight: '700', color: '#0f1111', margin: '0 0 6px 0', lineHeight: '1.3', height: '40px', overflow: 'hidden', display: '-webkit-box', WebkitLineClamp: 2, WebkitBoxOrient: 'vertical' }}>
                    {item.name}
                  </h2>

                  {/* Render Categories & SubCategories strings straight from your DTO object parameters */}
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

                  {/* Bottom Pricing Row Layout Matrix */}
                  <div style={{ marginTop: 'auto', display: 'flex', alignItems: 'baseline', gap: '4px', borderTop: '1px solid #f3f3f3', paddingTop: '10px' }}>
                    <span style={{ fontSize: '0.75rem', color: '#0f1111', fontWeight: '500' }}>₹</span>
                    <span style={{ fontSize: '1.4rem', fontWeight: '700', color: '#0f1111' }}>
                      {Intl.NumberFormat('en-IN').format(item.price)}
                    </span>
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