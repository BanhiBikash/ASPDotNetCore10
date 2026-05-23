import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import logo from '../assets/Amazon-Logo.png'; // Reuses your logo asset

const Login = () => {
  // state variable manages which panel view layout is currently active
  const [isLoginView, setIsLoginView] = useState(true);

  // Form states tracking values
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    password: '',
    confirmPassword: ''
  });

  const handleInputChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (isLoginView) {
      console.log('Processing Login validation for:', formData.email);
    } else {
      console.log('Processing API Registration submission for:', formData.name);
    }
  };

  return (
    <div className="auth-page-container">
      {/* Centered Decorative Corporate Logo Branding */}
      <div className="auth-logo-header">
        <Link to="/">
          <img src={logo} alt="Amazon Logo Black" className="auth-brand-logo" />
        </Link>
      </div>

      {/* Main Authentication Workspace Panel Card */}
      <div className="auth-card-box">
        <h1 className="auth-card-title">
          {isLoginView ? 'Sign in' : 'Create account'}
        </h1>

        <form onSubmit={handleSubmit} className="auth-form-flow">
          {/* Registration Mode Only Field: Full Name */}
          {!isLoginView && (
            <div className="auth-input-group">
              <label htmlFor="name">Your name</label>
              <input
                type="text"
                id="name"
                name="name"
                placeholder="First and last name"
                value={formData.name}
                onChange={handleInputChange}
                required
              />
            </div>
          )}

          {/* Core Identification Field: E-mail address */}
          <div className="auth-input-group">
            <label htmlFor="email">Email</label>
            <input
              type="email"
              id="email"
              name="email"
              value={formData.email}
              onChange={handleInputChange}
              required
            />
          </div>

          {/* Secure Credential Verification Entry Field */}
          <div className="auth-input-group">
            <label htmlFor="password">Password</label>
            <input
              type="password"
              id="password"
              name="password"
              placeholder={!isLoginView ? 'At least 6 characters' : ''}
              value={formData.password}
              onChange={handleInputChange}
              required
            />
          </div>

          {/* Registration Mode Only Field: Confirm Password Validation Check */}
          {!isLoginView && (
            <div className="auth-input-group">
              <label htmlFor="confirmPassword">Password again</label>
              <input
                type="password"
                id="confirmPassword"
                name="confirmPassword"
                value={formData.confirmPassword}
                onChange={handleInputChange}
                required
              />
            </div>
          )}

          {/* Contextual Submit Action Button */}
          <button type="submit" className="auth-action-btn-gold">
            {isLoginView ? 'Continue' : 'Create your Amazon account'}
          </button>
        </form>

        {/* Legal Disclaimer Framework Terms Segment */}
        <p className="auth-legal-disclaimer">
          By continuing, you agree to AmazonWeb's Clone{' '}
          <a href="#conditions">Conditions of Use</a> and{' '}
          <a href="#privacy">Privacy Notice</a>.
        </p>

        {/* Dynamic Context Interception Segment Toggles */}
        {isLoginView ? (
          <div className="auth-toggle-context-tray">
            <div className="auth-divider-break">
              <h5>New to Amazon?</h5>
            </div>
            <button 
              type="button" 
              className="auth-secondary-create-btn"
              onClick={() => setIsLoginView(false)}
            >
              Create your Amazon account
            </button>
          </div>
        ) : (
          <div className="auth-toggle-context-tray style-inline">
            <p>
              Already have an account?{' '}
              <span className="auth-switch-link" onClick={() => setIsLoginView(true)}>
                Sign in ➔
              </span>
            </p>
          </div>
        )}
      </div>

      {/* Discrete Bottom Footer Attachment Links */}
      <footer className="auth-minimal-footer">
        <div className="auth-footer-links">
          <a href="#conditions">Conditions of Use</a>
          <a href="#privacy">Privacy Notice</a>
          <a href="#help">Help</a>
        </div>
        <p className="auth-footer-copyright">
          &copy; 1996-{new Date().getFullYear()}, Amazon.com, Inc. or its affiliates
        </p>
      </footer>
    </div>
  );
};

export default Login;