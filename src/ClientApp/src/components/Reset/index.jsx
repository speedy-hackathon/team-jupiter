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
        <div style={{
            textAlign: 'center',
            marginBottom: `12px`,
        }}>
            <button onClick={handleResetButtonClick} use='default'>Начать сначала</button>
        </div>
    );
};
