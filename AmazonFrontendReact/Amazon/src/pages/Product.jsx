import React from 'react'
import { useParams } from 'react-router-dom'

const Product = () => {

  //fetch the product ID
  const {productID} = useParams()

  return (
    <div>Product:{productID}</div>
  )
}

export default Product