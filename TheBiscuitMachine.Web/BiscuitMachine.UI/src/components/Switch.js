import React from 'react';
import classNames from 'classnames';
import './Switch.css';

const Switch = props => {

    const onClasses = classNames({
        'btn': true,
        'btn-outline-dark': (!props.isOn || props.isPaused) && !props.isTurningOff,
        'btn-secondary': (props.isOn && !props.isPaused) || props.isTurningOff
    });

    const pauseClasses = classNames({
        'btn': true,
        'btn-outline-dark': props.isOn && !props.isPaused && !props.isTurningOff,
        'btn-secondary': !props.isOn || props.isPaused || props.isTurningOff
    });

    const offClasses = classNames({
        'btn': true,
        'btn-outline-dark': props.isOn && !props.isTurningOff,
        'btn-secondary': !props.isOn || props.isTurningOff
    });

    return <div className="switch btn-group float-left" role="group" aria-label="Basic example">
        <button type="button" className={onClasses} onClick={(!props.isOn && !props.isTurningOff) || props.isPaused ? props.on : null}>On</button>
        <button type="button" className={pauseClasses} onClick={props.isOn && !props.isPaused && !props.isTurningOff ? props.pause : null}>Pause</button>
        <button type="button" className={offClasses} onClick={props.isOn && !props.isTurningOff ? props.off : null}>Off</button>
    </div>;
}

export default React.memo(Switch);