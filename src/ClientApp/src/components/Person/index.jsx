import React from "react";
import styles from "./styles.module.css";
import {MAX_HEIGHT, MAX_WIDTH} from "../../consts/sizes";
import classNames from "classnames";

const healthState = {
    "healthy": "Healthy",
    "sick": "Sick",
    "dead": "Dead",
}

export default function Person({person, onClick}) {
    const x = (person.position.x / MAX_WIDTH) * 100;
    const y = (person.position.y / MAX_HEIGHT) * 100;
    const a = person.healthState;
    return (
        <div
            className={classNames(styles.root, {
                [styles.sickPerson]: person.healthState === healthState.sick,
                [styles.deadPerson]: person.healthState === healthState.dead,
                [styles.boredPerson]: person.isBored
            })}
            style={{left: `${x}%`, top: `${y}%`}}
            onClick={() => onClick(person.id)}
        />
    );
}
