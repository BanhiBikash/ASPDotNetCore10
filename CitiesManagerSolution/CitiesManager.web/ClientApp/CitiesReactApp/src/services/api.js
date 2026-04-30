import axios from "axios"

const API_BASE_URL = "https://localhost:5003/api/";

const apiClient = axios.create({baseURL: API_BASE_URL,
                                headers:{
                                    "content-type":"application/json"
                                }
})

const citiesService = {
    getAll : async ()=>{const response = await apiClient.get("v1/cities"); return response},
    getByID : async(id)=>{const response = await apiClient.get("v1/cities/"+id); return response},
    postData : async(formData)=>{const response = await apiClient.post("v1/cities",formData); return response},
    deleteByID : async(id)=>{const response = await apiClient.delete("v1/cities/"+id); return response},
    putByID: async(id,cityData)=>{const response = await apiClient.put("v1/cities/"+id,cityData); return response}
}

export default citiesService