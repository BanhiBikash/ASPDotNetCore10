import React, { useContext } from 'react'
import { useNavigate } from 'react-router-dom'
import CategorySubCategory from '../context/CategorySubCategory';

const CategoriesRow = () => {
  // Hook
  const navigate = useNavigate();
  
  // Destructure safely with a fallback default object
  const { category } = useContext(CategorySubCategory) || { category: {} };
  
  // Extract your explicit arrays with safe array fallback assignment
  const catArray = category?.catArray || [];
  const subCatArray = category?.subCatArray || [];

  return (
    <div className='categories'>
      {/* Heading */}
      <h2>Top Categories</h2>

      {/* Categories Row */}
      <div className="categories-row">
        {
          catArray.map(cat => (
            <div 
              className="categoryAndName" 
              key={cat.id || cat.categoryId}
              onClick={() => navigate(`/category/${cat.id || cat.categoryId}`)}
            >
              {/* Circular Category Icon Bounding Box */}
              <div className="category">
                <img 
                  src={cat.imageUrl || 'https://via.placeholder.com/150'} 
                  alt={cat.name} 
                  loading="lazy"
                />
              </div>
              
              {/* FIXED: Corrected <sapn> typo to a standard <span> tag */}
              <span>{cat.name}</span>
            </div>
          ))
        }
      </div>
    </div>
  )
}

export default CategoriesRow;