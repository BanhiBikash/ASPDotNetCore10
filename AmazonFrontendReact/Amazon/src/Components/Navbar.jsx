import React, { useContext, useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import logo from '../assets/Amazon-Logo.png';
import nav_icon from "../assets/hamburger.png";
import UserContext from '../context/UserContext'; 
import userLogo from "../assets/user.png";
import cartLogo from "../assets/cart.png";
import { useCart } from '../context/CartContext'; 

const Navbar = () => {
  const { user } = useContext(UserContext);
  const { cart } = useCart(); 
  const [category,setCategory] = useState([]);
  const [subCat,setSubCat] = useState([]);
  const navigate = useNavigate();

  //fetch categories
  useEffect(()=>{
  const fetchMetadata = async () => {
    console.log("getting categories....")
      try {
        const response = await api.get('v1/Products/GetCategories');
        const { categories, subCategories } = response.data;
        setCategory(categories)
        setSubCat(subCategories)
        console.log(category)
      }catch(e){
        console.log("Error: can't fetch category"+e)
      }
    }
  },[])

  // 🔍 Search bar input and category state filters
  const [searchQuery, setSearchQuery] = useState('');
  const [searchCategory, setSearchCategory] = useState('All');

  const getFirstName = () => {
    if (!user || !user.name) return 'Account';
    return user.name.split(' ')[0];
  };

  const getTotalCartCount = () => {
    if (!cart || !cart.cart || !Array.isArray(cart.cart)) return 0;
    return cart.cart.reduce((total, item) => total + (item.quantity || 0), 0);
  };

  const totalCount = getTotalCartCount();

  // ⚡ Handle Form Submit Navigation 
  const handleSearchSubmit = (e) => {
    e.preventDefault();
    if (!searchQuery.trim() && searchCategory === 'All') return;

    // Directs query parameters right into your responsive products listing catalog route space
    const params = new URLSearchParams();
    if (searchQuery.trim()) params.append('q', searchQuery.trim());
    if (searchCategory !== 'All') params.append('category', searchCategory);

    navigate(`/product?${params.toString()}`);
  };

  async function fetchQueryResponse() {
    
    console.log("entered")

    //fetch the data according to SearchQuery
    // const response = await api.get(`/v1/Products/SearchProductsByName/${searchQuery}`)
    // if(response){
    //   console.log("data received in search")
    //   console.log(response)
    // }else{
    //   console.log("Data not received")
    // }
  }

  //active-search
  const activeSearch = async (e)=>{
    //set the input value
    setSearchQuery(e.target.value)
    const data = await fetchQueryResponse()
  }

  return (
    <div className="Navbar">
      
      {/* Brand Icon Layout Sections */}
      <div className='icons'>
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

      {/* 🔍 NEW: Amazon-Style Core Search Bar Container Engine */}
      <form className="nav-search-bar-container" onSubmit={handleSearchSubmit}>
        <select 
          className="nav-search-dropdown"
          value={searchCategory}
          onChange={(e) => setSearchCategory(e.target.value)}
        >
          <option value="All">All Categories</option>
          <option value="Electronics">Electronics</option>
          <option value="Apparel">Clothing & Apparel</option>
          <option value="Quad_item">Quad Items</option>
          <option value="HomeDecor">Home Decor</option>
        </select>
        
        <input 
          type="text" 
          className="nav-search-input" 
          placeholder="Search AmazonWeb..." 
          value={searchQuery}
          onChange={(e) => activeSearch(e)}
        />
        
        <button type="submit" className="nav-search-submit-btn">
          {/* Magnifying glass icon layout */}
          <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" width="20" height="20" fill="currentColor">
            <path d="M15.5 14h-.79l-.28-.27C15.41 12.59 16 11.11 16 9.5 16 5.91 13.09 3 9.5 3S3 5.91 3 9.5 5.91 16 9.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zm-6 0C7.01 14 5 11.99 5 9.5S7.01 5 9.5 5 14 7.01 14 9.5 11.99 14 9.5 14z"/>
          </svg>
        </button>
      </form>

      {/* Navigation Links Routing Container */}
      <div className="nav-links">
        <Link to="/" className="nav-items text-link-node">Home</Link>
        <Link to="/product" className="nav-items text-link-node">Shop Products</Link>
        
        {user && (
          <Link to="/add_product" className="nav-items text-link-node">Add Products</Link>
        )}

        {user ? (
          <Link to="/Account" className="nav-items-nav-account-link-profile">
            <span className="nav-profile-firstname">{getFirstName()}</span>
            <img className="nav-profile-arrow-icon" src={userLogo} alt="User" />
          </Link>
        ) : (
          <Link to="/login" className="nav-items text-link-node">Login</Link>
        )}

        <Link to="/Cart" className='cartLogo'>
          <div className="cart-icon-wrapper">
            <img src={cartLogo} alt="Cart Logo" />
            {totalCount > 0 && (
              <span className="nav-cart-badge-count">{totalCount}</span>
            )}
          </div>
        </Link>
      </div>
      
    </div>
  );
};

export default Navbar;