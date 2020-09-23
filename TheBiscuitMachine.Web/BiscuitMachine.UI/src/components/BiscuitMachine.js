import React, { useState, useEffect } from 'react';
import Conveyor from './Conveyor';
import MachineElement from './MachineElement';
import Switch from './Switch';
import MachineElements from './MachineElements';
import BiscuitBox from './BiscuitBox';
import useBiscuitMachineEvents from '../hooks/useBiscuitMachineEvents';
import { BiscuitMachineAPIClient } from '../BiscuitMachineAPIClient';
import Motor from './Motor';

const modelTemplate = {
    isOn: false,
    isPaused: false,
    temperature: 0,
    isOvenOn: false,
    isOvenHeated: false,
    isExtruderActive: false,
    isStamperActive: false,
    isMotorActive: false,
    isFinishingProduction: false,
    slots: ['Empty', 'Empty', 'Empty', 'Empty', 'Empty', 'Empty'],
    moveRatio: 0,
    collectedBiscuits: 0
};

export const BiscuitMachine = () => {

    const [model, setModel] = useState(modelTemplate);

    useEffect(() => {
        const loadState = async () => {
            const state = await BiscuitMachineAPIClient.getState();
            setModel(prevModel => {
                return {
                    ...prevModel,
                    isOn: state.isOn,
                    isPaused: state.isPaused,
                    isOvenHeated: state.isOvenHeated,
                    isFinishingProduction: !state.isOn && state.isProductionStarted && !state.isProductionFinished
                }
            });
        }
        loadState();
    }, []);

    useBiscuitMachineEvents(setModel);

    const turnOn = async () => {
        await BiscuitMachineAPIClient.turnOn();
        const collectedBiscuits = model.isOn ? model.collectedBiscuits : 0;
        setModel(prevModel => { return { ...prevModel, isOn: true, isPaused: false, isFinishingProduction: false, collectedBiscuits: collectedBiscuits } });
    }

    const pause = async () => {
        await BiscuitMachineAPIClient.pause();
        setModel(prevModel => { return { ...prevModel, isPaused: true } });
    }

    const turnOff = async () => {
        await BiscuitMachineAPIClient.turnOff();
        setModel(prevModel => { return { ...prevModel, isFinishingProduction: true, isPaused: false } });
    }

    return (
        <div className="container">
            <h1>The Biscuit Machine</h1>
            <MachineElements>
                <MachineElement name="Extruder" capacity="1" isOn={model.isExtruderActive}>
                    {model.isExtruderActive && <div>Extracting dough..</div>}
                </MachineElement>
                <MachineElement name="Stamper" capacity="1" isOn={model.isStamperActive}>
                    {model.isStamperActive && <div>Stamping biscuit..</div>}
                </MachineElement>
                <MachineElement empty capacity="1" />
                <MachineElement name="Oven" capacity="2" isOn={model.isOvenOn}>
                    {(model.isOn || model.isFinishingProduction) && <span>Temperature: {model.temperature}</span>}
                    {model.isOn && !model.isOvenHeated && <div className="float-right">Warming up..</div>}
                </MachineElement>
                <MachineElement empty capacity="1" />
            </MachineElements>
            <Conveyor slots={model.slots} ratio={model.moveRatio} />
            <Motor name="Motor" capacity="1" isOn={model.isMotorActive}></Motor>
            <Switch isOn={model.isOn}
                isPaused={model.isPaused}
                isTurningOff={model.isFinishingProduction}
                on={turnOn}
                pause={pause}
                off={turnOff} />
            <BiscuitBox totalCount={model.collectedBiscuits} />
        </div>
    );
}

export default BiscuitMachine;
