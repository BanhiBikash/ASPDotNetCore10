import React from "react";

const WarningDialog = ({ message, type, onConfirm, onCancel }) => {
  return (
    <div className="dialog-overlay">
      <div className="dialog-box">
        <p>{message}</p>

        {type === "notfound" ? (
          <button onClick={onCancel} className="dialog-button">
            OK
          </button>
        ) : (
          <div className="dialog-actions">
            <button onClick={onConfirm} className="dialog-button delete">
              Delete
            </button>
            <button onClick={onCancel} className="dialog-button cancel">
              Cancel
            </button>
          </div>
        )}
      </div>
    </div>
  );
};

export default WarningDialog;
