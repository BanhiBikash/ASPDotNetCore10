import React, { useContext } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import logo from '../assets/Amazon-Logo.png';
import nav_icon from "../assets/hamburger.png";
import UserContext from '../context/UserContext'; // 🌐 Import your global user context

const Navbar = () => {
  const { user, setUser } = useContext(UserContext); // Extract authentication state
  const navigate = useNavigate();

  const handleLogout = () => {
    // 1. Flush secure application tokens out of storage
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');

    // 2. Clear global user footprint context state
    setUser(null);

    console.log('Session destroyed. User logged out securely.');
    
    // 3. Redirect back to home
    navigate('/');
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

      {/* Navigation Links (Public & Protected E-Commerce Routes) */}
      <div className="nav-links">
        <Link to="/" className="nav-items">Home</Link>
        <Link to="/product" className="nav-items">Shop Products</Link>
        
        {/* 👑 Role-Protected Navigation: Only show "Add Products" link to logged-in Admin accounts */}
        {user && (
          <Link to="/add_product" className="nav-items">Add Products</Link>
        )}

        {/* 🔄 Dynamic Authentication Switch */}
        {user ? (
          <div className="nav-user-logout-wrapper">
            {/* Optional subtle greeting indicating active user context profile */}
            <span className="nav-user-greeting">Hello, {user.name || 'User'}</span>
            <button onClick={handleLogout} className="nav-logout-btn">
              Logout
            </button>
          </div>
        ) : (
          <Link to="/login" className="nav-items">Login / Register</Link>
        )}
      </div>
    </div>
  );
};

export default Navbar;