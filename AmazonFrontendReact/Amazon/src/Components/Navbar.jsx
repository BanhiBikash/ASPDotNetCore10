import React from 'react';
import { Link } from 'react-router-dom';
import logo from '../assets/Amazon-Logo.png';
import nav_icon from "../assets/hamburger.png";

const Navbar = () => {
  return (
    <div className="Navbar">

      <div className='icons'>
        {/* Mobile Responsive Menu Toggle Icon */}
        <img
          src={nav_icon}
          alt="Toggle Menu"
          className="Nav-linksLogo-mobile"
          onClick={() => console.log('Mobile menu toggled')}
        />

        {/* Brand / Logo Section */}
        <Link to="/" className="logo-link">
          <img className="logo" src={logo} alt="AmazonWeb Logo" />
        </Link>
      </div>

      {/* Navigation Links (Public E-Commerce Routes) */}
      <div className="nav-links">
        <Link to="/" className="nav-items">Home</Link>
        <Link to="/products" className="nav-items">Shop Products</Link>
        <Link to="/account/login" className="nav-items">Login / Register</Link>
      </div>
    </div>
  );
};

export default Navbar;