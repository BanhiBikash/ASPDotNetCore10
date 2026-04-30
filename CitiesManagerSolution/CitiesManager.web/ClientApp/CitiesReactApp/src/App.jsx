import Navbar from './components/Navbar'
import CityTable from "./Components/Table"
import './App.css'
import WarningDialog from './Components/WarningDialogBox'
import { useState } from 'react'

function App() {
    const [dialogConfig, setDialogConfig] = useState(null);

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
            <Navbar />
            <CityTable setDialogConfig={setDialogConfig} dialog={dialogConfig} />
        </>
    )
}

export default App
