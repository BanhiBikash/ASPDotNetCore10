import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import logo from '../assets/Amazon-Logo.png';

const Login = () => {
  // Master switch toggling between standalone card components
  const [isLoginView, setIsLoginView] = useState(true);

  return (
    <div className="auth-page-container">
      {/* Centered Amazon Logo Header */}
      <div className="auth-logo-header">
        <Link to="/">
          <img src={logo} alt="Amazon Logo Black" className="auth-brand-logo" />
        </Link>
      </div>

      {/* Conditionally mount the completely independent form blocks */}
      {isLoginView ? (
        <LoginCard switchToRegister={() => setIsLoginView(false)} />
      ) : (
        <RegisterCard switchToLogin={() => setIsLoginView(true)} />
      )}

      {/* Shared Authentication Footer Links */}
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

/* ==========================================================================
   📦 STANDALONE COMPONENTS 1: LOGIN CARD
   ========================================================================== */
const LoginCard = ({ switchToRegister }) => {
  const [loginData, setLoginData] = useState({
    email: '',
    password: '',
    stayLoggedIn: false
  });

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setLoginData({
      ...loginData,
      [name]: type === 'checkbox' ? checked : value
    });
  };

  const handleLoginSubmit = (e) => {
    e.preventDefault();
    console.log('Sending Login Payload to .NET API:', loginData);
  };

  return (
    <div className="auth-card-box">
      <h1 className="auth-card-title">Sign in</h1>
      
      <form onSubmit={handleLoginSubmit} className="auth-form-flow">
        <div className="auth-input-group">
          <label htmlFor="login-email">Email</label>
          <input
            type="email"
            id="login-email"
            name="email"
            value={loginData.email}
            onChange={handleChange}
            required
          />
        </div>

        <div className="auth-input-group">
          <label htmlFor="login-password">Password</label>
          <input
            type="password"
            id="login-password"
            name="password"
            value={loginData.password}
            onChange={handleChange}
            required
          />
        </div>

        <div className="auth-checkbox-group">
          <input
            type="checkbox"
            id="login-stay"
            name="stayLoggedIn"
            checked={loginData.stayLoggedIn}
            onChange={handleChange}
          />
          <label htmlFor="login-stay">Keep me signed in</label>
        </div>

        <button type="submit" className="auth-action-btn-gold">Continue</button>
      </form>

      <p className="auth-legal-disclaimer">
        By continuing, you agree to AmazonWeb's Clone <a href="#conditions">Conditions of Use</a> and <a href="#privacy">Privacy Notice</a>.
      </p>

      <div className="auth-toggle-context-tray">
        <div className="auth-divider-break">
          <h5>New to Amazon?</h5>
        </div>
        <button type="button" className="auth-secondary-create-btn" onClick={switchToRegister}>
          Create your Amazon account
        </button>
      </div>
    </div>
  );
};

/* ==========================================================================
   📦 STANDALONE COMPONENT 2: REGISTER CARD (FIXED GRID LAYOUT)
   ========================================================================== */
const RegisterCard = ({ switchToLogin }) => {
  const [registerData, setRegisterData] = useState({
    email: '',
    firstName: '',
    lastName: '',
    dateOfBirth: '',
    password: '',
    confirmPassword: '',
    gender: '0',
    stayLoggedIn: false
  });

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData({
      ...registerData,
      [name]: type === 'checkbox' ? checked : value
    });
  };

  const handleRegisterSubmit = (e) => {
    e.preventDefault();
    const registerPayload = {
      ...registerData,
      dateOfBirth: registerData.dateOfBirth ? registerData.dateOfBirth : null,
      gender: parseInt(registerData.gender, 10)
    };
    console.log('Sending RegisterDTO Payload to .NET API:', registerPayload);
  };

  return (
    <div className="auth-card-box register-card-wide">
      <h1 className="auth-card-title">Create account</h1>
      
      <form onSubmit={handleRegisterSubmit} className="auth-form-flow">
        
        {/* ROW 1: First Name & Last Name sitting cleanly side-by-side */}
        <div className="auth-form-row-grid">
          <div className="auth-input-group">
            <label htmlFor="firstName">First name</label>
            <input
              type="text"
              id="firstName"
              name="firstName"
              maxLength={50}
              value={registerData.firstName}
              onChange={handleChange}
              required
            />
          </div>
          <div className="auth-input-group">
            <label htmlFor="lastName">Last name</label>
            <input
              type="text"
              id="lastName"
              name="lastName"
              maxLength={50}
              value={registerData.lastName}
              onChange={handleChange}
              required
            />
          </div>
        </div>

        {/* ROW 2: Date of Birth & Gender Selection alignment */}
        <div className="auth-form-row-grid">
          <div className="auth-input-group">
            <label htmlFor="dateOfBirth">Date of birth</label>
            <input
              type="date"
              id="dateOfBirth"
              name="dateOfBirth"
              value={registerData.dateOfBirth}
              onChange={handleChange}
            />
          </div>
          <div className="auth-input-group">
            <label htmlFor="gender">Gender</label>
            <select
              id="gender"
              name="gender"
              value={registerData.gender}
              onChange={handleChange}
              className="auth-select-field"
              required
            >
              <option value="0">Male</option>
              <option value="1">Female</option>
              <option value="2">Other</option>
            </select>
          </div>
        </div>

        {/* Full Width Standard Inputs */}
        <div className="auth-input-group">
          <label htmlFor="reg-email">Email</label>
          <input
            type="email"
            id="reg-email"
            name="email"
            value={registerData.email}
            onChange={handleChange}
            required
          />
        </div>

        <div className="auth-input-group">
          <label htmlFor="reg-password">Password</label>
          <input
            type="password"
            id="reg-password"
            name="password"
            minLength={6}
            maxLength={100}
            placeholder="At least 6 characters"
            value={registerData.password}
            onChange={handleChange}
            required
          />
        </div>

        <div className="auth-input-group">
          <label htmlFor="confirmPassword">Password again</label>
          <input
            type="password"
            id="confirmPassword"
            name="confirmPassword"
            value={registerData.confirmPassword}
            onChange={handleChange}
            required
          />
        </div>

        <div className="auth-checkbox-group">
          <input
            type="checkbox"
            id="reg-stay"
            name="stayLoggedIn"
            checked={registerData.stayLoggedIn}
            onChange={handleChange}
          />
          <label htmlFor="reg-stay">Keep me signed in</label>
        </div>

        <button type="submit" className="auth-action-btn-gold">Add Account</button>
      </form>

      <p className="auth-legal-disclaimer">
        By creating an account, you agree to AmazonWeb's Clone <a href="#conditions">Conditions of Use</a> and <a href="#privacy">Privacy Notice</a>.
      </p>

      <div className="auth-toggle-context-tray style-inline">
        <p>
          Already have an account?{' '}
          <span className="auth-switch-link" onClick={switchToLogin}>
            Sign in ➔
          </span>
        </p>
      </div>
    </div>
  );
};

export default Login;