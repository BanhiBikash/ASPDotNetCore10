import React, { useState, useEffect } from 'react';
import api from '../api/axiosConfig';

const ProductAdd = () => {
  // 🎯 Maps perfectly to your ASP.NET Core ProductAddRequest properties
  const [productData, setProductData] = useState({
    name: '',
    price: '',
    stock: '',
    description: '',
    category: '',    
    subCategory: ''  
  });

  const [categories, setCategories] = useState([]);
  const [allSubCategories, setAllSubCategories] = useState([]);
  const [filteredSubCategories, setFilteredSubCategories] = useState([]);
  
  const [thumbnailFile, setThumbnailFile] = useState(null);
  const [uiStatus, setUiStatus] = useState({ loading: false, fetchLoading: true, success: null, error: null });

  // 📡 Fetch enum lookups on mount
  useEffect(() => {
    const fetchMetadata = async () => {
      try {
        const response = await api.get('/Products/GetCategories');
        const { categories, subCategories } = response.data;
        
        setCategories(categories);
        setAllSubCategories(subCategories);

        if (categories.length > 0) {
          const firstCatId = categories[0].id.toString();
          setProductData(prev => ({ ...prev, category: firstCatId }));
          filterSubCategoriesList(firstCatId, categories, subCategories);
        }

        setUiStatus(prev => ({ ...prev, fetchLoading: false }));
      } catch (err) {
        console.error('Failed fetching product enum metadata:', err);
        setUiStatus(prev => ({ 
          ...prev, 
          fetchLoading: false, 
          error: 'Failed to synchronize category mappings from server.' 
        }));
      }
    };

    fetchMetadata();
  }, []);

  // 🔍 Dynamic Prefix Translation Dictionary to perfectly link your C# Enums
  const filterSubCategoriesList = (categoryIdStr, currentCats, currentSubs) => {
    const numericId = parseInt(categoryIdStr, 10);
    const selectedCategoryName = currentCats.find(c => c.id === numericId)?.name;

    if (!selectedCategoryName) {
      setFilteredSubCategories([]);
      setProductData(prev => ({ ...prev, category: categoryIdStr, subCategory: '' }));
      return;
    }

    // 🛠️ Bridge the naming gap between your singular and plural naming formats
    const prefixMap = {
      'Mobiles': 'Mobile_',
      'Laptops': 'Laptop_',
      'Fashion': 'Fashion_',
      'Books': 'Book_',
      'HomeAppliances': 'HomeAppliance_',
      'Furniture': 'Furniture_',
      'Toys': 'Toy_',
      'Sports': 'Sports_',
      'Beauty': 'Beauty_',
      'Health': 'Health_',
      'Groceries': 'Grocery_',
      'Pets': 'Pet_',
      'Automotive': 'Automotive_',
      'Jewelry': 'Jewelry_',
      'Shoes': 'Shoe_',
      'Stationary': 'Stationary_',
      'Common': 'Common'
    };

    // Get correct search prefix based on dictionary mapping, fallback to name string if missing
    const matchPrefix = prefixMap[selectedCategoryName] || `${selectedCategoryName}_`;
    
    // Filter out subcategory enums matching our translated root prefix
    const filtered = currentSubs.filter(sub => sub.name.startsWith(matchPrefix));
    
    setFilteredSubCategories(filtered);

    // Default choice is left empty ("None") when swapping parent categories
    setProductData(prev => ({
      ...prev,
      category: categoryIdStr,
      subCategory: '' 
    }));
  };

  const handleCategoryChange = (e) => {
    const targetCategoryId = e.target.value;
    filterSubCategoriesList(targetCategoryId, categories, allSubCategories);
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setProductData({ ...productData, [name]: value });
  };

  const handleFileChange = (e) => {
    if (e.target.files && e.target.files[0]) {
      setThumbnailFile(e.target.files[0]);
    }
  };

  const handleFormSubmit = async (e) => {
    e.preventDefault();
    if (!productData.category) {
      setUiStatus(prev => ({ ...prev, error: 'Please select a valid primary category.' }));
      return;
    }

    setUiStatus(prev => ({ ...prev, loading: true, success: null, error: null }));

    const multiPartForm = new FormData();
    const stockCount = parseInt(productData.stock, 10) || 0;

    multiPartForm.append('Name', productData.name);
    multiPartForm.append('Price', parseInt(productData.price, 10) || 0);
    multiPartForm.append('Stock', stockCount);
    multiPartForm.append('InStock', stockCount > 0);
    multiPartForm.append('Description', productData.description);
    multiPartForm.append('Category', parseInt(productData.category, 10));
    
    // Only append SubCategory if it's explicitly set and not "None"
    if (productData.subCategory && productData.subCategory !== '') {
      multiPartForm.append('SubCategory', parseInt(productData.subCategory, 10));
    }
    
    if (thumbnailFile) {
      multiPartForm.append('Thumbnail', thumbnailFile);
    }

    try {
      await api.post('/Products', multiPartForm);
      setUiStatus(prev => ({ ...prev, loading: false, success: 'Product successfully saved to database catalog!', error: null }));
      
      // Clear standard text values while keeping dropdown structural positions intact
      setProductData(prev => ({ ...prev, name: '', price: '', stock: '', description: '', subCategory: '' }));
      setThumbnailFile(null);
      document.getElementById('thumbnail').value = '';
    } catch (err) {
      console.error(err);
      const backendErrorMessage = err.response?.data?.message || err.message || 'Server data injection failure.';
      setUiStatus(prev => ({ ...prev, loading: false, success: null, error: backendErrorMessage }));
    }
  };

  if (uiStatus.fetchLoading) {
    return (
      <div className="auth-page-container" style={{ justifyContent: 'center' }}>
        <p style={{ fontSize: '0.9rem', color: '#666' }}>Synchronizing Enum structures from ASP.NET API stream...</p>
      </div>
    );
  }

  return (
    <div className="auth-page-container">
      <div className="auth-card-box register-card-wide">
        <h1 className="auth-card-title">Product Management Hub</h1>
        <p style={{ fontSize: '0.8rem', color: '#666', margin: '-10px 0 15px 0' }}>Admin Console: Add catalog inventory rows</p>

        {uiStatus.success && <div className="admin-status-alert success">{uiStatus.success}</div>}
        {uiStatus.error && <div className="admin-status-alert error">{uiStatus.error}</div>}

        <form onSubmit={handleFormSubmit} className="auth-form-flow">
          
          <div className="auth-input-group">
            <label htmlFor="name">Product Name</label>
            <input
              type="text"
              id="name"
              name="name"
              maxLength={100}
              value={productData.name}
              onChange={handleChange}
              required
            />
          </div>

          <div className="auth-form-row-grid">
            <div className="auth-input-group">
              <label htmlFor="price">Price (INR - Integer)</label>
              <input
                type="number"
                id="price"
                name="price"
                min="0"
                value={productData.price}
                onChange={handleChange}
                required
              />
            </div>
            <div className="auth-input-group">
              <label htmlFor="stock">Stock Quantity</label>
              <input
                type="number"
                id="stock"
                name="stock"
                min="0"
                value={productData.stock}
                onChange={handleChange}
                required
              />
            </div>
          </div>

          <div className="auth-form-row-grid">
            {/* Primary Category Dropdown Menu */}
            <div className="auth-input-group">
              <label htmlFor="category">Category</label>
              <select
                id="category"
                name="category"
                value={productData.category}
                onChange={handleCategoryChange}
                className="auth-select-field"
                required
              >
                {categories.map(cat => (
                  <option key={cat.id} value={cat.id}>{cat.name}</option>
                ))}
              </select>
            </div>
            
            {/* Contextually Synchronized SubCategory Selector */}
            <div className="auth-input-group">
              <label htmlFor="subCategory">Sub-Category (Optional)</label>
              <select
                id="subCategory"
                name="subCategory"
                value={productData.subCategory}
                onChange={handleChange}
                className="auth-select-field"
                disabled={filteredSubCategories.length === 0}
              >
                <option value="">None / No Subcategory</option>
                
                {filteredSubCategories.map(sub => {
                  // Strips away layout formatting labels on screen: "Mobile_Smartphones" -> displays "Smartphones"
                  const displayLabel = sub.name.includes('_') ? sub.name.split('_')[1] : sub.name;
                  return <option key={sub.id} value={sub.id}>{displayLabel}</option>;
                })}
              </select>
            </div>
          </div>

          <div className="auth-input-group">
            <label htmlFor="thumbnail">Product Thumbnail Image File</label>
            <input
              type="file"
              id="thumbnail"
              name="thumbnail"
              accept="image/*"
              onChange={handleFileChange}
              required
            />
          </div>

          <div className="auth-input-group">
            <label htmlFor="description">Product Specification Description</label>
            <textarea
              id="description"
              name="description"
              rows="4"
              className="admin-textarea-field"
              value={productData.description}
              onChange={handleChange}
              required
            />
          </div>

          <button 
            type="submit" 
            className="auth-action-btn-gold" 
            disabled={uiStatus.loading}
            style={{ padding: '8px 0', fontWeight: '700' }}
          >
            {uiStatus.loading ? 'Uploading Data Streams...' : 'Publish Product to Catalog'}
          </button>
        </form>
      </div>
    </div>
  );
};

export default ProductAdd;