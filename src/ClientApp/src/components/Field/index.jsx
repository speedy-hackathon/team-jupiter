import React from "react";
import styles from "./styles.module.css";
import Person from "../Person";
import House from "../House";

export default function Field({ map, people, onClick }) {
  return (
    <div className={styles.root}>
      {map.map((item, i) => {
        const coordinates = item.coordinates.leftTopCorner;
        return <House key={i} x={coordinates.x} y={coordinates.y} building={item}/>
      })}
      {people.map((item) => (
        <Person person={item} key={item.id} onClick={onClick} />
      ))}
    </div>
  );
}
