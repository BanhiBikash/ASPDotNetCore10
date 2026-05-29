import React, { useContext, useEffect, useState } from "react"
import CartContext from "./CartContext"
import UserContext from "./UserContext"
import api from "../api/axiosConfig"

const CartContextProvider = ({ children }) => {

    const [cart, setCart] = useState({
        cart: [],
        isBusy: false
    })

    const { user } = useContext(UserContext)

    // 🎯 FIXED: Added 'async' so that 'await api.get' compiles cleanly
    async function fetchInitialCartData() {   

        // Cart is busy fetching initial data
        setCart(prev => { return { ...prev, isBusy: true } })

        // Either user is not set or email Id is not there, use local storage
        if (!user || !user.email) {
            const localCart = localStorage.getItem('guest_cart')

            if (localCart) {  
                // 🎯 FIXED: Moved try/catch to sit completely around JSON.parse()
                try {
                    const parsedLocalCart = JSON.parse(localCart)
                    setCart({ 
                        cart: Array.isArray(parsedLocalCart) ? parsedLocalCart : [], 
                        isBusy: false 
                    })
                }
                catch (e) {
                    console.log("failed to parse local cart!", e)
                    setCart({ cart: [], isBusy: false })
                }
            } else {  
                setCart({ cart: [], isBusy: false })
            }
        }
        // User is found, look for DB cart
        else {
            try {
                const response = await api.get('/v1/Cart');

                // Targeting the exact "items" root property key from your C# DTO response payload
                const backendCartArray = response.data?.items || [];

                setCart({
                    cart: Array.isArray(backendCartArray) ? backendCartArray : [],
                    isBusy: false
                });
            } catch (err) {
                console.error("Failed to sync authenticated cart array from backend service layer:", err);
                setCart({ cart: [], isBusy: false });
            }
        }
    }

    useEffect(function () { 
        fetchInitialCartData() 
    }, [user])

    return (
        <CartContext.Provider value={{ cart, setCart }}>
            {children}
        </CartContext.Provider>
    )
}

export default CartContextProvider