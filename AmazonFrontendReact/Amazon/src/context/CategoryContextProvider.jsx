import React, {useState,useEffect} from 'react'
import CategorySubCategory from './CategorySubCategory'
import api from '../api/axiosConfig'

const CategoryContextProvider = ({children}) => {

    const [category, setCategory] = useState({category:[],SubCategory:[]})

    //fetch categories
    useEffect(function()
    {
        // async function to do it
        async function fetchCategory(){
            const response = api.get('');
        }

        fetchCategory();
    },
    []);

  return (
    <CategorySubCategory.Provider value={category,setCategory} >
        {children}
    </CategorySubCategory.Provider>
  )
}

export default CategoryContextProvider