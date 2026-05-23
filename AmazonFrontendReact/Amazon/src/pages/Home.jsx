import React from "react";
import { Link } from "react-router-dom";

const Home = () => {
  return (
    <div className="home-page-container">
      {/* 1. HERO BANNER BACKGROUND CAROUSEL */}
      <div className="hero-banner-slider">
        <img 
          src="https://images-eu.ssl-images-amazon.com/images/G/31/img24/Sports/November/GW/Herobar/DesktopHero_3000x1200._CB542387654_.jpg" 
          alt="Amazon Feature Deals Banner" 
          className="hero-image"
        />
        <div className="hero-gradient-overlay" />
      </div>

      {/* 2. OVERLAPPING OVERVIEW HUB MATRIX GRID */}
      <div className="home-content-grid">
        
        {/* Card 1: Quad Component layout */}
        <div className="product-card-container">
          <h2 className="card-title">Revamp your home | Up to 60% off</h2>
          <div className="quad-image-grid">
            <div className="quad-item">
              <img src="https://images-eu.ssl-images-amazon.com/images/G/31/IMG15/Irfan/Gatewway/Electronics/372x232_1._CB413583643_.jpg" alt="Home Decor" />
              <span>Cushion covers & bedsheets</span>
            </div>
            <div className="quad-item">
              <img src="https://images-eu.ssl-images-amazon.com/images/G/31/IMG15/Irfan/Gatewway/Electronics/372x232_2._CB413583643_.jpg" alt="Lighting" />
              <span>Lighting solutions</span>
            </div>
            <div className="quad-item">
              <img src="https://images-eu.ssl-images-amazon.com/images/G/31/IMG15/Irfan/Gatewway/Electronics/372x232_3._CB413583643_.jpg" alt="Storage" />
              <span>Storage & organizers</span>
            </div>
            <div className="quad-item">
              <img src="https://images-eu.ssl-images-amazon.com/images/G/31/IMG15/Irfan/Gatewway/Electronics/372x232_4._CB413583643_.jpg" alt="Laundry" />
              <span>Laundry baskets</span>
            </div>
          </div>
          <Link to="/product" className="card-explore-link">See more deals</Link>
        </div>

        {/* Card 2: Single Large Item display */}
        <div className="product-card-container">
          <h2 className="card-title">Latest Devices | Fire TV & Echo</h2>
          <div className="single-image-wrapper">
            <img 
              src="https://images-eu.ssl-images-amazon.com/images/G/31/img22/Devices/GW/PC_CC_1x._CB625983421_.jpg" 
              alt="Amazon Echo Devices" 
              className="single-card-img"
            />
          </div>
          <Link to="/product" className="card-explore-link">Explore smart features</Link>
        </div>

        {/* Card 3: Quad Component layout (Appliances) */}
        <div className="product-card-container">
          <h2 className="card-title">Appliances for your home</h2>
          <div className="quad-image-grid">
            <div className="quad-item">
              <img src="https://images-eu.ssl-images-amazon.com/images/G/31/IMG15/Irfan/Gatewway/Appliances/Quad_AirCond._CB624538902_.jpg" alt="AC" />
              <span>Air Conditioners</span>
            </div>
            <div className="quad-item">
              <img src="https://images-eu.ssl-images-amazon.com/images/G/31/IMG15/Irfan/Gatewway/Appliances/Quad_Fridges._CB624538902_.jpg" alt="Fridges" />
              <span>Refrigerators</span>
            </div>
            <div className="quad-item">
              <img src="https://images-eu.ssl-images-amazon.com/images/G/31/IMG15/Irfan/Gatewway/Appliances/Quad_Microwaves._CB624538902_.jpg" alt="Microwaves" />
              <span>Microwaves</span>
            </div>
            <div className="quad-item">
              <img src="https://images-eu.ssl-images-amazon.com/images/G/31/IMG15/Irfan/Gatewway/Appliances/Quad_Washing._CB624538902_.jpg" alt="Washing Machine" />
              <span>Washing Machines</span>
            </div>
          </div>
          <Link to="/product" className="card-explore-link">Check dynamic pricing</Link>
        </div>

        {/* Card 4: Quick Sign-In Module Callout */}
        <div className="product-card-container gateway-auth-promo">
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
        </div>

      </div>

      {/* 3. BREAKOUT SINGLE ROW: WIDE DEALS SLIDER TIER */}
      <div className="wide-deals-strip-container">
        <h2 className="strip-section-title">Today's Deals | Handpicked Top Offers</h2>
        <div className="horizontal-scroll-row">
          <div className="deal-thumb-box">
            <img src="https://images-eu.ssl-images-amazon.com/images/G/31/img23/Wireless/Samsung/CatPage/Tiles/New/M34._CB573981290_.png" alt="Phone" />
            <span className="deal-badge">Up to 35% Off</span>
            <p className="deal-desc">Samsung Galaxy Series</p>
          </div>
          <div className="deal-thumb-box">
            <img src="https://images-eu.ssl-images-amazon.com/images/G/31/img22/Wired/Headphones/Boat/Dual_Desktop._CB612349503_.jpg" alt="Audio" />
            <span className="deal-badge">Min 50% Off</span>
            <p className="deal-desc">boAt Rockerz & Audio Audio</p>
          </div>
          <div className="deal-thumb-box">
            <img src="https://images-eu.ssl-images-amazon.com/images/G/31/img21/Computers/Laptops/Gateway/March/Corei5._CB580392011_.jpg" alt="Laptops" />
            <span className="deal-badge">Up to ₹20,000 Off</span>
            <p className="deal-desc">Core i5 Thin & Light Laptops</p>
          </div>
          <div className="deal-thumb-box">
            <img src="https://images-eu.ssl-images-amazon.com/images/G/31/img23/Fashion/Event/Gateway/Deals/3._CB574920401_.jpg" alt="Footwear" />
            <span className="deal-badge">40% - 70% Off</span>
            <p className="deal-desc">Sports Running Shoes catalog</p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Home;