import React from 'react'
import { useNavigate } from 'react-router-dom'

const CategoriesRow = () => {

    //hook
    const navigate = useNavigate();

  return (
    <div className='categories'>
        {/* heading */}
        <h2>Top Categories</h2>

        {/* categories row */}
        <div className="categories-row">
            {/* circular category icon */}
            <div className="category"></div>
            <div className="category"></div>
            <div className="category"></div>
            <div className="category"></div>
            <div className="category"></div>
            <div className="category"></div>
            <div className="category"></div>
            <div className="category"></div>
            <div className="category"></div>
            <div className="category"></div>
        </div>
    </div>
  )
}

export default CategoriesRow