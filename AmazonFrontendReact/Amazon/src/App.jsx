import './App.css'
import { BrowserRouter, createBrowserRouter, createRoutesFromElements, Route, Router, RouterProvider } from 'react-router-dom';
import { Amazon } from './Amazon'
import Home from "./pages/Home"
import Login from "./pages/Login"
import Product from "./pages/Product"
import ProductAdd from './pages/AddProduct';
import Account from './pages/Account';
import Account_Update from './pages/Account_Update';
import Cart from "./pages/Cart"
import Checkout from './pages/Checkout';

function App() {
  // 1. Declare your layout routes cleanly inside your browser router creation hook
  const router = createBrowserRouter(
    createRoutesFromElements(
      <Route path="/" element={<Amazon />}>
        <Route index element={<Home />} />                {/* Matches layout root "/" */}
        <Route path="account" element={<Account />} />                {/* Matches layout root "/" */}
        <Route path="Account_Update" element={<Account_Update />} />                {/* Matches layout root "/" */}
        <Route path="login" element={<Login />} />        {/* Matches "/login" */}
        <Route path="product" element={<Product />} />    {/* Matches "/product" */}
        <Route path="add_product" element={<ProductAdd />} />    {/* Matches "/add_product" */}
        <Route path="Cart" element={<Cart />} />    {/* Matches "/add_product" */}
        <Route path="Checkout" element={<Checkout />} /> {/* The direct, express gateway page bypass */}
      </Route>
    )
  );

  // 2. Return the standalone provider passing your config down using the router property 
  return <RouterProvider router={router} />;
}

export default App
