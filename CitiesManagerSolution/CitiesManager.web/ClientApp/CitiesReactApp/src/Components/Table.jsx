import React, { useEffect, useState } from "react";
import axios from "axios";
import TableRow from "./TableRow";
import citiesService from "../services/api";
import PostCityDialogBox from "./PostCityDialogBox";
import EditDialog from "./EditDialog";

function CityTable(prop) {

    const [tableData, setTableData] = React.useState([])
    const [postDialog, setPostDialog] = React.useState(null)
    const [editDialog, setEditDialog] = React.useState(false)
    const [formData, setFormData] = React.useState({ cityId: "", cityName: "" })

    //render the data
    const renderData = (data) => {
        setTableData(data)
    }

    //data fetch
    const fetchData = async () => {
        try {
            console.log('fetching...')
            const response = await citiesService.getAll();
            renderData(response.data);
        } catch (e) {
            console.error("Error fetching data:", e);
        }
    }

    //initial fetch
    useEffect(() => {
        fetchData()
    }, [])

    //button actions
    //get single city
    const getByID = async (id) => {
        console.log("getting By Id" + id)
        const response = await citiesService.getByID(id);
        //giving in array form
        renderData([response.data])
    }

    //delete by ID
    const deleteByID = async (id) => {
        const response = await citiesService.getByID(id)

        if (response.data == null) //No city data found
        {
            console.log("city not found")

            prop.setDialogConfig({ message: "City not found!", type: "notfound", onCancel: () => { prop.setDialogConfig(null) } })

        }
        else    //City Data found, delete WARNING!
        {
            console.log("confirm!")
            prop.setDialogConfig({
                message: "Are you sure you want to delete this city?", type: "confirm",
                onCancel: () => { prop.setDialogConfig(null) },
                onConfirm: async () => {
                    const responseDelete = await citiesService.deleteByID(id)
                    console.log(responseDelete)
                    //check if deleted
                    if (responseDelete.status === 200 || responseDelete.status === 204) {    //city deleted
                        prop.setDialogConfig({ message: "City successfully deleted", type: "notfound", onCancel: () => { prop.setDialogConfig(null) } })
                        const reloadResponse = await citiesService.getAll();
                        renderData(reloadResponse.data)
                    } else {    //city not deleted
                        prop.setDialogConfig({ message: "City not deleted", type: "notfound", onCancel: () => { prop.setDialogConfig(null) } })
                    }
                }
            })
        }
    }

    const addCity = () => {
        console.log("open post")
        setPostDialog({ type: "post", cityData: { cityId: "", cityName: "" } })
    }

    //edit
    const edit = async (cityId, cityName) => {
        setFormData({ cityId: cityId, cityName: cityName })
        setEditDialog(true)
    }

    const handleUpdate = async (event) => {
        console.log("edit start")
        event.preventDefault()
        const response = await citiesService.putByID(formData.cityId, formData);
        console.log(response.status)
        if (response.status === 200 || response.status === 204) {
            setEditDialog(false)
            prop.setDialogConfig({ message: "City Data updated", type: "notfound", onCancel: () => { prop.setDialogConfig(null) } })
            fetchData()
        } else {
            setEditDialog(false)
            prop.setDialogConfig({ message: "City Data update failed!", type: "notfound", onCancel: () => { prop.setDialogConfig(null) } })
        }
    }

    return (
        <div className="city-table-container">
            <button className='buttonEnLarge' onClick={addCity}>Add City</button>
            <table className="city-table">
                <thead>
                    <tr>
                        <th>City ID</th>
                        <th>City Name</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {tableData.map(function (city) { return (<TableRow cityId={city.cityId} cityName={city.cityName} getByID={getByID} edit={edit} deleteByID={deleteByID} />) })}
                </tbody>
            </table>
            {/* PostCity-DialogBox */}
            {postDialog && <PostCityDialogBox setPostDialog={setPostDialog} renderData={renderData} type={postDialog.type} cityData={postDialog.cityData} setPostDialog={setPostDialog} />}
            {/*EditDialgBox*/}
            {editDialog && <EditDialog formData={formData} setFormData={setFormData} handleUpdate={handleUpdate} setEditDialog={setEditDialog} />}
        </div>
    );
}

export default CityTable;

