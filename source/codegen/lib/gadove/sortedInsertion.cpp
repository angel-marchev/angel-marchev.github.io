/*
 * sortedInsertion.cpp
 *
 * Code generation for function 'sortedInsertion'
 *
 */

/* Include files */
#include "rt_nonfinite.h"
#include "gadove.h"
#include "sortedInsertion.h"

/* Function Definitions */
void sortedInsertion(double x, int ix, double b[5], int *nb, int idx[5])
{
  int ja;
  int jb;
  int jc;
  if (*nb == 0) {
    *nb = 1;
    idx[0] = ix;
    b[0] = x;
  } else if ((x >= b[0]) || rtIsNaN(x)) {
    if ((*nb > 1) && (!(x >= b[*nb - 1])) && (!rtIsNaN(x))) {
      ja = 1;
      jb = *nb;
      while (ja < jb) {
        jc = ja + ((jb - ja) >> 1);
        if (jc == ja) {
          ja = jb;
        } else if ((x >= b[jc - 1]) || rtIsNaN(x)) {
          ja = jc;
        } else {
          jb = jc;
        }
      }

      if (*nb < 5) {
        (*nb)++;
      }

      jc = ja + 1;
      for (jb = *nb; jb >= jc; jb--) {
        b[jb - 1] = b[jb - 2];
        idx[jb - 1] = idx[jb - 2];
      }

      b[ja - 1] = x;
      idx[ja - 1] = ix;
    } else {
      if (*nb < 5) {
        (*nb)++;
        b[*nb - 1] = x;
        idx[*nb - 1] = ix;
      }
    }
  } else {
    if (*nb < 5) {
      (*nb)++;
    }

    for (jb = *nb; jb >= 2; jb--) {
      idx[jb - 1] = idx[jb - 2];
      b[jb - 1] = b[jb - 2];
    }

    b[0] = x;
    idx[0] = ix;
  }
}

/* End of code generation (sortedInsertion.cpp) */
