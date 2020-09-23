import { useEffect } from 'react';
import { HubConnectionBuilder } from '@aspnet/signalr';

const useBiscuitMachineEvents = setModel => {
    useEffect(() => {
        const connection = new HubConnectionBuilder()
            .withUrl("/hub/BiscuitMachine")
            .build();

        connection.on('state_changed', data => {
            const res = JSON.parse(data);
            console.log(res.State);
            setModel(prevModel => {
                return {
                    ...prevModel,
                    isOn: res.State.IsOn,
                    isPaused: res.State.IsPaused,
                    isOvenHeated: res.State.IsOvenHeated,
                    isFinishingProduction: !res.State.IsOn && res.State.IsProductionStarted && !res.State.IsProductionFinished
                }
            });
        });

        connection.on('oven_on', () => {
            setModel(prevModel => {
                return { ...prevModel, isOvenOn: true }
            });
        });

        connection.on('oven_off', () => {
            setModel(prevModel => { return { ...prevModel, isOvenOn: false } });
        });

        connection.on('temperature_changed', data => {
            const res = JSON.parse(data);
            setModel(prevModel => { return { ...prevModel, temperature: res.Temperature } });
        });

        connection.on('oven_heated', () => {
            setModel(prevModel => { return { ...prevModel, isOvenHeated: true } });
        });

        connection.on('extruder_activated', () => {
            setModel(prevModel => { return { ...prevModel, isExtruderActive: true } });
        });

        connection.on('biscuit_extracted', () => {
            setModel(prevModel => {
                var slotsCopy = [...prevModel.slots];
                slotsCopy[0] = 'Extracted';
                return { ...prevModel, slots: slotsCopy, isExtruderActive: false }
            });
        });

        connection.on('stamper_activated', () => {
            setModel(prevModel => { return { ...prevModel, isStamperActive: true } });
        });

        connection.on('biscuit_stamped', () => {
            setModel(prevModel => {
                var slotsCopy = [...prevModel.slots];
                slotsCopy[1] = 'Stamped';
                return { ...prevModel, slots: slotsCopy, isStamperActive: false }
            });
        });

        connection.on('biscuit_baked', () => {
            setModel(prevModel => {
                var slotsCopy = [...prevModel.slots];
                if (slotsCopy[3] === 'Stamped') {
                    slotsCopy[3] = 'HalfBaked';
                }
                if (slotsCopy[4] === 'HalfBaked') {
                    slotsCopy[4] = 'Baked';
                }
                return { ...prevModel, slots: slotsCopy }
            });
        });

        connection.on('biscuit_collected', data => {
            const res = JSON.parse(data);
            setModel(prevModel => { return { ...prevModel, collectedBiscuits: res.TotalBiscuitsCollected } });
        });

        connection.on('motor_activated', () => {
            setModel(prevModel => { return { ...prevModel, isMotorActive: true } });
            setTimeout(() => setModel(prevModel => { return { ...prevModel, isMotorActive: false } }), 200);
        });

        connection.on('conveyor_moved', data => {
            const res = JSON.parse(data);
            setModel(prevModel => {
                return { ...prevModel, moveRatio: parseFloat(res.ConveyorPositionRatio) }
            });
        });

        connection.on('conveyor_position_reached', data => {
            const res = JSON.parse(data);
            setModel(prevModel => { return { ...prevModel, slots: res.Slots, moveRatio: 0 } });
        });

        connection.on('production_finished', () => {
            setModel(prevModel => {
                return { ...prevModel, isOn: false, isPaused: false, isOvenOn: false }
            });
        });

        connection.start()
            .then(() => console.log('Connection started!'))
            .catch(err => console.log('Error while establishing connection :('));

    }, [setModel]);
}

export default useBiscuitMachineEvents;