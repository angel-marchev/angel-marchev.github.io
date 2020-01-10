/*
 * gadove_emxutil.h
 *
 * Code generation for function 'gadove_emxutil'
 *
 */

#ifndef GADOVE_EMXUTIL_H
#define GADOVE_EMXUTIL_H

/* Include files */
#include <stddef.h>
#include <stdlib.h>
#include "rtwtypes.h"
#include "gadove_types.h"

/* Function Declarations */
extern void emxEnsureCapacity_real_T(emxArray_real_T *emxArray, int oldNumel);
extern void emxFree_real_T(emxArray_real_T **pEmxArray);
extern void emxInit_real_T(emxArray_real_T **pEmxArray, int numDimensions);

#endif

/* End of code generation (gadove_emxutil.h) */
