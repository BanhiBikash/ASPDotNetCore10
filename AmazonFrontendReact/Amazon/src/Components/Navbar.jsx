import React from 'react';
import { Link } from 'react-router-dom';
import logo from '../assets/Amazon.jpg';

const Navbar = () => {
  return (
    <div className="Navbar">
      {/* Brand / Logo Section */}
      <Link to="/" className="logo-link">
        <img className="logo" src={logo} alt="AmazonWeb Logo" />
      </Link>

      {/* Navigation Links (Public E-Commerce Routes) */}
      <div className="nav-links">
        <Link to="/" className="nav-items">Home</Link>
        <Link to="/products" className="nav-items">Shop Products</Link>
        <Link to="/account/login" className="nav-items">Login / Register</Link>
      </div>

      {/* Mobile Responsive Menu Toggle Icon */}
      <img 
        src="/assets/menu-icon.svg" 
        alt="Toggle Menu" 
        className="Nav-linksLogo-mobile" 
        onClick={() => console.log('Mobile menu toggled')}
      />
    </div>
  );
};

export default Navbar;