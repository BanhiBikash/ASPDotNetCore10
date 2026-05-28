import React, { useContext } from 'react';
import { Link } from 'react-router-dom';
import logo from '../assets/Amazon-Logo.png';
import nav_icon from "../assets/hamburger.png";
import UserContext from '../context/UserContext'; 
import userLogo from "../assets/user.png";
import cartLogo from "../assets/cart.png";
import useCart from '../context/CartContext'; 

const Navbar = () => {
  const { user } = useContext(UserContext);
  
  // 🛒 Destructuring cart safely using your default hook export
  const { cart } = useCart(); 

  // Extract just the first name from user profile state if available
  const getFirstName = () => {
    if (!user || !user.name) return 'Account';
    return user.name.split(' ')[0];
  };

  // 🧮 Compute the total sum of item quantities safely
  const getTotalCartCount = () => {
    if (!cart || !Array.isArray(cart)) return 0;
    return cart.reduce((total, item) => total + (item.quantity || 0), 0);
  };

  const totalCount = getTotalCartCount();

  return (
    <div className="Navbar">
      
      {/* Brand Icon Layout Sections */}
      <div className='icons'>
        {/* Mobile Responsive Menu Toggle Icon */}
        <img
          src={nav_icon}
          alt="Toggle Menu"
          className="Nav-linksLogo-mobile"
          onClick={() => console.log('Mobile menu toggled')}
        />
        
        <Link to="/" className="logo-link">
          <img className="logo" src={logo} alt="AmazonWeb Logo" />
        </Link>
      </div>

      {/* Navigation Links Routing Container */}
      <div className="nav-links">
        <Link to="/" className="nav-items">Home</Link>
        <Link to="/product" className="nav-items">Shop Products</Link>
        
        {/* Admin Capability Gate */}
        {user && (
          <Link to="/add_product" className="nav-items">Add Products</Link>
        )}

        {/* 🔄 Dynamic User Identity Node */}
        {user ? (
          <Link to="/Account" className="nav-items-nav-account-link-profile">
            <span className="nav-profile-firstname">{getFirstName()}</span>
            <img className="nav-profile-arrow-icon" src={userLogo} alt="User" />
          </Link>
        ) : (
          <Link to="/login" className="nav-items">Login / Register</Link>
        )}

        {/* 🛒 Cart Anchor with Clean Conditional Badge Rendering */}
        <Link to="/Cart" className='cartLogo'>
          <img src={cartLogo} alt="Cart Logo" />
          
          {/* 🎯 Elegant Short-circuit: Only renders the span node if items exist, avoiding structural layout issues */}
          {totalCount > 0 && (
            <span>{totalCount}</span>
          )}
        </Link>
      </div>
      
    </div>
  );
};

export default Navbar;