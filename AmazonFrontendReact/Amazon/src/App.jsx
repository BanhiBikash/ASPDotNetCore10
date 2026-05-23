import './App.css'
import { BrowserRouter, createBrowserRouter, createRoutesFromElements, Route, Router, RouterProvider } from 'react-router-dom';
import { Amazon } from './Amazon'
import Home from "./pages/Home"
import Login from "./pages/Login"
import Product from "./pages/Product"
import ProductAdd from './pages/AddProduct';

function App() {
  // 1. Declare your layout routes cleanly inside your browser router creation hook
  const router = createBrowserRouter(
    createRoutesFromElements(
      <Route path="/" element={<Amazon />}>
        <Route index element={<Home />} />                {/* Matches layout root "/" */}
        <Route path="login" element={<Login />} />        {/* Matches "/login" */}
        <Route path="product" element={<Product />} />    {/* Matches "/product" */}
        <Route path="add_product" element={<ProductAdd />} />    {/* Matches "/add_product" */}
      </Route>
    )
  );

  // 2. Return the standalone provider passing your config down using the router property 
  return <RouterProvider router={router} />;
}

export default App
