import React from 'react';
import {userActionUrl} from '../../consts/urls';

export const ResetButton = () => {
    const handleResetButtonClick = () => {
        fetch(`${userActionUrl}reset`, {
            method: `POST`,
            headers: {
                "Content-Type": "application/json",
              },
        });
    }

    return (
        <div>
            <button onClick={handleResetButtonClick}>Reset</button>
        </div>
    );
};
