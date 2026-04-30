import React from 'react'

const TableRow = (props) => {

    return (
        <tr key={props.cityId}>
            <td>{props.cityId}</td>
            <td>{props.cityName}</td>
            <td>
                <button onClick={function(){props.getByID(props.cityId)}} className='buttonEnLarge'>Enlarge</button>
                <button onClick={function(){props.edit(props.cityId,props.cityName)}} className='buttonEdit'>Edit</button>
                <button onClick={function(){props.deleteByID(props.cityId)}} className='buttonDelete'>Delete</button>
            </td>
        </tr>
    )
}

export default TableRow