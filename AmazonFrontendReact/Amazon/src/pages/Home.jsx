import React, { useContext, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import api from "../api/axiosConfig";
import UserContext from "../context/UserContext";

const Home = () => {

  //get user
  const { user } = useContext(UserContext)

  const [quad, setQuad] = useState([])
  const [quad2, setQuad2] = useState([])
  const [row, setRow] = useState([])

  //banner images
  const bannerSlides = [
    {
      id: 1,
      image: "https://images.unsplash.com/photo-1531297484001-80022131f5a1?q=80&w=1500&h=500&fit=crop&crop=top",
      alt: "New Tech Vanguard Arrival Deals"
    },
    {
      id: 2,
      image: "https://images.unsplash.com/photo-1441986300917-64674bd600d8?q=80&w=1500&h=500&fit=crop&crop=center",
      alt: "Summer Fashion Collection Sale"
    },
    {
      id: 3,
      image: "https://images.unsplash.com/photo-1607082348824-0a96f2a4b9da?q=80&w=1500&h=500&fit=crop&crop=center",
      alt: "Mega Flash Clearance Event"
    }
  ];

  async function getQuad() {
    //get quad items
    const quad_items = await api.get('/v1/Products/category/Furniture');

    if (quad_items != null) {
      console.log("data received")
    }
    console.log(quad_items.data)
    setQuad(quad_items.data)
  }

  async function getQuad2() {
    //get quad items
    const quad_items = await api.get('/v1/Products/category/HomeAppliances');

    if (quad_items != null) {
      console.log("data received")
    }
    console.log(quad_items.data)
    setQuad2(quad_items.data)
  }

  async function getRow() {

    //get itens
    const response = await api.get('/v1/Products/category/Mobiles');

    if (response != null) {
      console.log("data received mobile")
    }
    console.log(response.data)
    setRow(response.data)
  }

  //load the items on render
  useEffect(function () {
    getQuad(); getQuad2(); getRow();
  }, [])

  return (
    <div className="home-page-container">
      {/* 1. HERO BANNER BACKGROUND CAROUSEL */}
      <div className="hero-banner-slider">
        {/* {Array.isArray(bannerSlides) && bannerSlides.map(item => {
          return (
            <img
              src="https://images-eu.ssl-images-amazon.com/images/G/31/img24/Sports/November/GW/Herobar/DesktopHero_3000x1200._CB542387654_.jpg"
              alt="Amazon Feature Deals Banner"
              className="hero-image"
            />
          )
        })} */}
        <div className="hero-gradient-overlay" />
      </div>

      {/* 2. OVERLAPPING OVERVIEW HUB MATRIX GRID */}
      <div className="home-content-grid">

        {/* Card 1: Quad Component layout */}
        <div className="product-card-container">
          <h2 className="card-title">Revamp your home | Up to 60% off</h2>

          {/* go through first 4 items and display them */}
          <div className="quad-image-grid">
            {Array.isArray(quad) && quad.slice(0, 4).map((item) => {
              // Optional: strips out database prefix underscores if present (e.g., "Decor_Mirrors" -> "Mirrors")
              const displaySubCategory = item.subCategory && item.subCategory.includes('_')
                ? item.subCategory.split('_')[1]
                : item.subCategory;

              return (
                <div className="quad-item" key={item.id || item.productId}>
                  <img
                    src={item.imageUrl}
                    alt={item.name || "Home Decor"}
                    onError={(e) => {
                      e.target.onerror = null;
                      e.target.src = 'https://placehold.co/300?text=Image+Load+Error';
                    }}
                  />
                  <span>{displaySubCategory || 'View Item'}</span>
                </div>
              );
            })}
          </div>
          <Link to="/product" className="card-explore-link">See more deals</Link>
        </div>

        {/* Card 2: Single Large Item display */}
        <div className="product-card-container">
          <h2 className="card-title">Latest Devices | Fire TV & Echo</h2>
          <div className="single-image-wrapper">
            <img
              src="https://helios-i.mashable.com/imagery/comparisons/00fRDHtInqzkLrGIuQ4dwtw-item2.fit_lim.size_1028x578.v1742572179.png"
              alt="Amazon Echo Devices"
              className="single-card-img"
            />
          </div>
          <Link to="/product" className="card-explore-link">Explore smart features</Link>
        </div>

        {/* Card 1: Quad Component layout */}
        <div className="product-card-container">
          <h2 className="card-title">Revamp your home | Up to 60% off</h2>

          {/* go through first 4 items and display them */}
          <div className="quad-image-grid">
            {Array.isArray(quad2) && quad2.slice(0, 4).map((item) => {
              // Optional: strips out database prefix underscores if present (e.g., "Decor_Mirrors" -> "Mirrors")
              const displaySubCategory = item.subCategory && item.subCategory.includes('_')
                ? item.subCategory.split('_')[1]
                : item.subCategory;

              return (
                <div className="quad-item" key={item.id || item.productId}>
                  <img
                    src={item.imageUrl}
                    alt={item.name || "Home Decor"}
                    onError={(e) => {
                      e.target.onerror = null;
                      e.target.src = 'https://placehold.co/300?text=Image+Load+Error';
                    }}
                  />
                  <span>{displaySubCategory || 'View Item'}</span>
                </div>
              );
            })}
          </div>
          <Link to="/product" className="card-explore-link">See more deals</Link>
        </div>

        {/* Card 4: Quick Sign-In Module Callout - if no user */}
        {!{ user } && <div className="product-card-container gateway-auth-promo">
          <div className="promo-inner-block">
            <h2 className="card-title">Sign in for your best experience</h2>
            <Link to="/login" className="amazon-primary-btn">Sign in securely</Link>
          </div>
          <div className="promo-banner-footer-img">
            <img
              src="https://images-eu.ssl-images-amazon.com/images/G/31/img19/AmazonPay/Avatar/GWBanners/AmazonPay_Short_Grid._CB443928132_.jpg"
              alt="Amazon Pay Integration Promo"
            />
          </div>
        </div>}

      </div>

      {/* 3. BREAKOUT SINGLE ROW: WIDE DEALS SLIDER TIER */}
      <div className="wide-deals-strip-container">
        <h2 className="strip-section-title">Today's Deals | Handpicked Top Offers</h2>
        <div className="horizontal-scroll-row">
          {Array.isArray(row) && row.map(item => {
            return (<div className="deal-thumb-box">
              <img src={item.imageUrl} alt={item.name} />
              <span className="deal-badge">Up to 35% Off</span>
              <p className="deal-desc">{item.name}</p>
            </div>)
          })}
        </div>
      </div>
    </div>
  );
};

export default Home;