import React from 'react'

const EditDialog = (props) => {

    const handleFormChange = (event)=>{
        const {name,value} = event.target
        props.setFormData((prev)=>{return {...prev,[name]:value}})
    }

    const onCancel = ()=>{
        props.setEditDialog(false)
    }

  return (
    <form className="dialog-overlay" onSubmit={function(event){props.handleUpdate(event)}}>
      <div className="dialog-box">

        <h1>Update City</h1>
        <div className="dialog-form">
          <label>
            City ID:
            <input
              type="text"
              name="cityId"
              value={props.formData.cityId}
              onChange={(event) => handleFormChange(event)}
              disabled
            />
          </label>
          <label>
            City Name:
            <input
              type="text"
              name="cityName"
              value={props.formData.cityName}
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
  )
}

export default EditDialog
