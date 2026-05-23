import React from "react";
import Navbar from "./Components/Navbar";
import Footer from "./Components/Footer"
import { Outlet } from "react-router-dom";

export const Amazon=()=>{
    return(
    <div className="app-layout-wrapper">
      {/* Locked to viewport top */}
      <Navbar />
      
      {/* Dynamic page context insertion area */}
      <main className="main-content-fluid">
        <Outlet />
      </main>
      
      {/* Base alignment ground layer */}
      <Footer />
    </div>
    )
}