import type { ICoordinateSimple } from "./dto.model";

export const SAFE_DELTA_AREA: Array<ICoordinateSimple> = [
  {
    x: 0,
    y: 0,
  },
  {
    x: -1,
    y: 0,
  },
  {
    x: 1,
    y: 0,
  },
  {
    x: 0,
    y: -1,
  },
  {
    x: 0,
    y: 1,
  },
  {
    x: -1,
    y: -1,
  },
  {
    x: 1,
    y: 1,
  },
  {
    x: 1,
    y: -1,
  },
  {
    x: -1,
    y: 1,
  },
];
export const HORIZONTAL_AREA_LENGTH = 10;
export const VERTICAL_AREA_LENGTH = 10;
