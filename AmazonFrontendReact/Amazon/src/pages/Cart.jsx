import React,{createContext, useContext, useEffect, useState} from 'react'

export const CartContext = createContext(null)

export const CartContextProvider = ({children})=>{

    const[cart,setCart] = useState([])

    return(
        <CartContext.Provider value={{cart,setCart}}>
            {children}
        </CartContext.Provider>
    )
}

export default function useCart(){
    return(useContext(CartContext))
}
 