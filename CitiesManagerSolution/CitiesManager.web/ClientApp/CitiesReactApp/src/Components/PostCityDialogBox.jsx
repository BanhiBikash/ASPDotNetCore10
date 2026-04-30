import React,{useState} from 'react'
import citiesService from '../services/api'
import WarningDialog from './WarningDialogBox';

const PostCityDialogBox = (props) => {

    const [dialogConfig, setDialogConfig] = useState(null);

    //close dialog box
    const onCancel = ()=>{
        //set form data null
        setFormData(null)
        //close dialog box
        props.setPostDialog(null)
    }

    const [formData,setFormData] = useState({cityId:"",cityName:""})

    //handle formData
    const handleFormChange = (event)=>{
        const {name,value} = event.target
        console.log(name, value)
        setFormData((prev)=>{return {...prev,[name]:value}})
    }

    const handleUpdate = async ()=>{
    }

    const handlePost = async (event)=>{
      event.preventDefault();
        console.log("posting")
        const response  = await citiesService.postData(formData)
      console.log(response)
        if(response.status!=201){
            //close dialog box -post
            props.setPostDialog(null)
            setDialogConfig({message:"Failed to add city!", type:"notfound", onCancel:()=>{setDialogConfig(null)}})
        }else{
            console.log("city added")
            props.setPostDialog(null)
            setDialogConfig({message:"City Added",type:"notfound", onCancel:()=>{setDialogConfig(null)}})
        }
    }

  return (
    <>
            {dialogConfig && (
                <WarningDialog
                    message={dialogConfig.message}
                    type={dialogConfig.type}
                    onConfirm={dialogConfig.onConfirm}
                    onCancel={dialogConfig.onCancel}
                />
            )}
    <form className="dialog-overlay" onSubmit={function(event){(props.type==="post")?handlePost(event):handleUpdate(event)}}>
      <div className="dialog-box">

        {/* heading choice */}
        {(props.type==="post")?
            <h1>Add City</h1>:
            <h1>Update City</h1>
        }
        <div className="dialog-form">
          <label>
            City ID:
            <input
              type="text"
              name="cityId"
              value={formData.cityId}
              onChange={(event) => handleFormChange(event)}
              disabled={props.type==="update"}
            />
          </label>
          <label>
            City Name:
            <input
              type="text"
              name="cityName"
              value={formData.cityName}
              onChange={(event) => handleFormChange(event)}
            />
          </label>
        </div>
        <div className="dialog-actions">
          <button type="submit" className="dialog-button save">
            Save
          </button>
          <button onClick={onCancel} className="dialog-button cancel">
            Cancel
          </button>
        </div>
      </div>
    </form>
    </>
  );
}

export default PostCityDialogBox