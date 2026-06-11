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
            //make request
            const response = await api.get('v1/Products/GetCategories');
            
            if(response==null){
                console.log("failed to fetch category and sub category")
            }else{
                const {cat, subCat} = response.data;
                setCategory({category:cat, SubCategory: subCat})
                console.log(category)
            }
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