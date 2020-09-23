import React from 'react';
import './Motor.css';

const Motor = props => {
    return <div className="motor mt-3 float-left border border-dark rounded-circle">
        Motor
        <div className="rounded-circle motor-indicator" style={{ backgroundColor: props.isOn ? 'green' : 'red' }}></div>
    </div>;
}

export default React.memo(Motor);