import React, { useContext } from 'react';
import { Link } from 'react-router-dom';
import logo from '../assets/Amazon-Logo.png';
import nav_icon from "../assets/hamburger.png";
import UserContext from '../context/UserContext'; // 🌐 Global authentication context
import userLogo from "../assets/user.png";

const Navbar = () => {
  const { user } = useContext(UserContext); // Track logged-in account structure

  // Extract just the first name from user profile state if available
  const getFirstName = () => {
    if (!user || !user.name) return 'Account';
    return user.name.split(' ')[0];
  };

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

      {/* Navigation Links */}
      <div className="nav-links">
        <Link to="/" className="nav-items">Home</Link>
        <Link to="/product" className="nav-items">Shop Products</Link>
        
        {/* Admin Capability Gate */}
        {user && (
          <Link to="/add_product" className="nav-items">Add Products</Link>
        )}

        {/* 🔄 Dynamic Identity Node */}
        {user ? (
          <Link to="./Account" className="nav-items-nav-account-link-profile">
            <span className="nav-profile-firstname">{getFirstName()}</span>
            {/* 🎯 Inline layout representation of ">" Arrow Icon */}
            <img className="nav-profile-arrow-icon" src={userLogo} alt="User" />
          </Link>
        ) : (
          <Link to="/login" className="nav-items">Login / Register</Link>
        )}
      </div>
    </div>
  );
};

export default Navbar;