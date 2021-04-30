import React from "react";
import styles from "./styles.module.css";
import { MAX_HEIGHT, MAX_WIDTH } from "../../consts/sizes";

export default function Person({ person, onClick }) {
  const x = (person.position.x / MAX_WIDTH) * 100;
  const y = (person.position.y / MAX_HEIGHT) * 100;
  return (
    <div
      className={styles.root}
      style={{ left: `${x}%`, top: `${y}%` }}
      onClick={() => onClick(person.id)}
    />
  );
}
