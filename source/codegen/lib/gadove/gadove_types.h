/*
 * gadove_types.h
 *
 * Code generation for function 'gadove'
 *
 */

#ifndef GADOVE_TYPES_H
#define GADOVE_TYPES_H

/* Include files */
#include "rtwtypes.h"

/* Type Definitions */
struct emxArray_real_T
{
  double *data;
  int *size;
  int allocatedSize;
  int numDimensions;
  boolean_T canFreeData;
};

#endif

/* End of code generation (gadove_types.h) */
