import React from 'react';
import { Button } from '@skbkontur/react-ui';
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
            <Button onClick={handleResetButtonClick} use='default'>Начать сначала</Button>
        </div>
    );
};
