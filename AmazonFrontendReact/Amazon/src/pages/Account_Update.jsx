import React, { useState, useContext, useEffect } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import UserContext from '../context/UserContext';
import axios from 'axios'; // Or import your configured 'api' instance
import api from '../api/axiosConfig';

const Account_Update = () => {
  const { user, setUser } = useContext(UserContext);
  const navigate = useNavigate();

  // State for form tracking
  const [profileData, setProfileData] = useState({
    address: '',
    city: '',
    state: '',
    postalCode: '',
    country: ''
  });
  
  const [selectedFile, setSelectedFile] = useState(null);
  const [previewUrl, setPreviewUrl] = useState('');
  const [uiStatus, setUiStatus] = useState({ loading: false, success: null, error: null });

  // Hydrate fields if user context data already has address properties
  useEffect(() => {
    if (user) {
      setProfileData({
        address: user.address || '',
        city: user.city || '',
        state: user.state || '',
        postalCode: user.postalCode || '',
        country: user.country || ''
      });
      if (user.profileImageUrl) {
        setPreviewUrl(user.profileImageUrl);
      }
    }
  }, [user]);

  // Route fallback guard for unauthenticated users
  if (!user) {
    return (
      <div className="account-dashboard-fallback">
        <h2>Please log in to alter security profiles.</h2>
        <Link to="/login" className="auth-action-btn-gold" style={{ padding: '8px 24px' }}>Sign In</Link>
      </div>
    );
  }

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setProfileData(prev => ({ ...prev, [name]: value }));
  };

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setSelectedFile(file);
      setPreviewUrl(URL.createObjectURL(file)); // Dynamic local layout avatar preview
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setUiStatus({ loading: true, success: null, error: null });

    // Pack multipart payload fields
    const multiPartForm = new FormData();
    multiPartForm.append('address', profileData.address);
    multiPartForm.append('city', profileData.city);
    multiPartForm.append('state', profileData.state);
    multiPartForm.append('postalCode', profileData.postalCode);
    multiPartForm.append('country', profileData.country);

    if (selectedFile) {
      multiPartForm.append('profileImage', selectedFile);
    }

    try {
      // Adjust the URL endpoint string to match your routing setups
      const response = await api.put('/v1/Account/UpdateProfile', multiPartForm, {
        headers: { 
          'Content-Type': 'multipart/form-data',
          'Authorization': `Bearer ${localStorage.getItem('token')}`
        }
      });

      setUiStatus({ 
        loading: false, 
        success: 'Your personal delivery records have been saved successfully!', 
        error: null 
      });

      // Synchronize backend response changes with global React tracking context
      setUser(prev => ({
        ...prev,
        address: response.data.address,
        city: response.data.city,
        state: response.data.state,
        postalCode: response.data.postalCode,
        country: response.data.country,
        profileImageUrl: response.data.profileImageUrl
      }));

    } catch (err) {
      console.error(err);
      const errorMessage = err.response?.data?.message || err.message || 'Profile alteration communication failure.';
      setUiStatus({ loading: false, success: null, error: errorMessage });
    }
  };

  return (
    <div className="auth-page-container">
      <div className="auth-logo-header">
        <Link to="/account" className="account-back-breadcrumb">
          ‹ Back to Dashboard
        </Link>
      </div>

      <div className="auth-card-box register-card-wide">
        <h2 className="auth-card-title">Login & Security</h2>
        <p className="profile-subtitle-context">Update your global shipping vectors and identity footprint below.</p>

        {uiStatus.success && <div className="admin-status-alert success">{uiStatus.success}</div>}
        {uiStatus.error && <div className="admin-status-alert error">{uiStatus.error}</div>}

        <form onSubmit={handleSubmit} className="auth-form-flow">
          
          {/* Avatar Graphic Segment */}
          <div className="profile-avatar-management-node">
            <div className="profile-avatar-preview-shell">
              {previewUrl ? (
                <img src={previewUrl} alt="User Avatar" className="profile-avatar-circle" />
              ) : (
                <div className="profile-avatar-placeholder">👤</div>
              )}
            </div>
            <div className="auth-input-group" style={{ flex: 1 }}>
              <label htmlFor="avatar-upload">Profile Photo</label>
              <input 
                id="avatar-upload" 
                type="file" 
                accept="image/*" 
                onChange={handleFileChange} 
              />
            </div>
          </div>

          <div className="auth-input-group">
            <label>Street Address</label>
            <input 
              type="text" 
              name="address" 
              value={profileData.address} 
              onChange={handleInputChange} 
              placeholder="123 Amazon Way, Apt 4B" 
            />
          </div>

          <div className="auth-form-row-grid">
            <div className="auth-input-group">
              <label>City</label>
              <input 
                type="text" 
                name="city" 
                value={profileData.city} 
                onChange={handleInputChange} 
              />
            </div>
            <div className="auth-input-group">
              <label>State / Region</label>
              <input 
                type="text" 
                name="state" 
                value={profileData.state} 
                onChange={handleInputChange} 
              />
            </div>
          </div>

          <div className="auth-form-row-grid">
            <div className="auth-input-group">
              <label>Postal Code</label>
              <input 
                type="text" 
                name="postalCode" 
                value={profileData.postalCode} 
                onChange={handleInputChange} 
              />
            </div>
            <div className="auth-input-group">
              <label>Country</label>
              <input 
                type="text" 
                name="country" 
                value={profileData.country} 
                onChange={handleInputChange} 
              />
            </div>
          </div>

          <button 
            type="submit" 
            className="auth-action-btn-gold" 
            disabled={uiStatus.loading}
            style={{ marginTop: '12px' }}
          >
            {uiStatus.loading ? 'Saving Changes...' : 'Save Changes'}
          </button>
        </form>
      </div>
    </div>
  );
};

export default Account_Update;